using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Accounts;

public class RegisterViewModel
{
    [Required(ErrorMessage = "O nome � obrigat�rio.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O email � obrigat�rio.")]
    [EmailAddress(ErrorMessage = "O email � inv�lido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "A senha � obrigat�rio.")]
    public string Password { get; set; }

}