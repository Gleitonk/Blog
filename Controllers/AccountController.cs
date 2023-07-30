using System.Text.RegularExpressions;
using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.Services;
using Blog.ViewModels;
using Blog.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace Blog.Controllers;

[ApiController]
public class AccountController : ControllerBase
{

    [HttpPost("v1/accounts")]
    public async Task<IActionResult> Create(
        [FromBody] RegisterViewModel model,
        [FromServices] EmailService emailService,
        [FromServices] BlogDataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var user = new User
        {
            Name = model.Name,
            Email = model.Email,
            Slug = model.Email.Replace("@", "-").Replace(".", "-"),
            PasswordHash = PasswordHasher.Hash(model.Password)
        };

        try
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            emailService.Send(
                user.Name,
                user.Email,
                "Bem vindo ao Blog!",
                $"Olá {user.Name.Split(" ")[0]}! 😎");

            return Ok(new ResultViewModel<dynamic>(new
            {
                user = user.Email,
                password = user.PasswordHash
            }));
        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<List<Category>>("ASDG98 - Este email já esta cadastrado."));
        }
        catch (Exception)
        {
            return StatusCode(400, new ResultViewModel<List<Category>>("SERH90 - Falha interna no servidor."));
        }
    }


    [HttpPost("v1/accounts/login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginViewModel model,
        [FromServices] BlogDataContext context,
        [FromServices] TokenService tokenService)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var user = await context
            .Users
            .AsNoTracking()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Email == model.Email);

        if (user == null)
            return StatusCode(401, new ResultViewModel<List<Category>>("Usuário ou senha inválidos."));

        if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
            return StatusCode(401, new ResultViewModel<List<Category>>("Usuário ou senha inválidos."));

        try
        {
            var token = tokenService.GenerateToken(user);
            return Ok(new ResultViewModel<string>(token, null));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("SERH91 - Falha interna no servidor."));
        }
    }

    [Authorize]
    [HttpPost("v1/accounts/upload-image")]
    public async Task<IActionResult> UploadImage(
        [FromBody] UploadImageViewModel model,
        [FromServices] BlogDataContext context)
    {
        var filename = $"{Guid.NewGuid().ToString()}.jpg";
        var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(model.Base64Image, "");

        var bytes = Convert.FromBase64String(data);

        try
        {
            await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{filename}", bytes);

        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("SERH92 - Falha interna no servidor."));
        }

        var user = await context
            .Users
            .FirstOrDefaultAsync(x => x.Id.ToString() == User.Identity.Name);

        if (user == null)
            return NotFound(new ResultViewModel<string>("Usuário não encontrado."));

        user.Image = $"http://localhost:3333/images/{filename}";

        try
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("SERH97 - Falha interna no servidor."));
        }

        return Ok(new ResultViewModel<string>("Imagem alterada com sucesso.", null));
    }
}