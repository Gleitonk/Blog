using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Accounts;

public class LoginViewModel
{
    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O email é inválido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatório.")]
    [StringLength(16, MinimumLength = 8, ErrorMessage = "A senha deve conter entre {2} e {1} caracteres.")]
    public string Password { get; set; }
}