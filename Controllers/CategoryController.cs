using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
public class CategoryController : ControllerBase
{
    [HttpGet("v1/categories")]
    public async Task<IActionResult> GetAsync(
        [FromServices] BlogDataContext context
    )
    {
        try
        {
            var categories = await context.Categories.ToListAsync();
            return Ok(new ResultViewModel<List<Category>>(categories));
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("QWE05 - N�o foi poss�vel buscar categorias."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("AWSE05 - Falha interna no servidor."));
        }
    }

    [HttpGet("v1/categories/{id:int}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] int id,
        [FromServices] BlogDataContext context
    )
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null) return NotFound(new ResultViewModel<Category>("Conte�do n�o encontrado."));
            return Ok(new ResultViewModel<Category>(category));
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new ResultViewModel<Category>("QWE06 - N�o foi poss�vel buscar categoria."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<Category>("AWSE06 - Falha interna no servidor."));
        }
    }

    [HttpPost("v1/categories")]
    public async Task<IActionResult> CreateAsync(
        [FromBody] EditorCategoryViewModel model,
        [FromServices] BlogDataContext context
    )
    {
        if (!ModelState.IsValid) return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

        try
        {
            var category = new Category
            {
                Id = 0,
                Name = model.Name,
                Slug = model.Slug.ToLower(),
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
            return Created($"v1/categories/{category.Id}", category);
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new ResultViewModel<Category>("QWE07 - N�o foi poss�vel incluir a categoria."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<Category>("AWSE07 - Falha interna no servidor."));
        }
    }

    [HttpPut("v1/categories/{id:int}")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] int id,
        [FromBody] Category model,
        [FromServices] BlogDataContext context
    )
    {
        if (!ModelState.IsValid) return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null) return NotFound(StatusCode(500, new ResultViewModel<Category>("N�o encontrado.")));

            category.Name = model.Name;
            category.Slug = model.Slug;

            context.Categories.Update(category);
            await context.SaveChangesAsync();

            return Ok(StatusCode(500, new ResultViewModel<Category>(category)));
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new ResultViewModel<Category>("QWE08 - N�o foi poss�vel atualizar a categoria."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<Category>("AWSE08 - Falha interna no servidor."));
        }
    }

    [HttpDelete("v1/categories/{id:int}")]
    public async Task<IActionResult> DeleteAsync(
       [FromRoute] int id,
       [FromServices] BlogDataContext context
)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null) return NotFound(StatusCode(500, new ResultViewModel<Category>("N�o encontrado.")));

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return Ok(StatusCode(500, new ResultViewModel<Category>(category)));
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new ResultViewModel<Category>("QWE09 - N�o foi poss�vel remover a categoria."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<Category>("AWSE09 - Falha interna no servidor."));
        }
    }

}
