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
    [StringLength(16, MinimumLength = 8, ErrorMessage = "A senha deve conter entre {2} e {1} caracteres.")]
    public string Password { get; set; }

}