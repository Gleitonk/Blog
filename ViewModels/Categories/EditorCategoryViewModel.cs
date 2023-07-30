using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Categories;

public class EditorCategoryViewModel
{
    [Required(ErrorMessage = "O nome � obrigat�rio.")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "O nome deve conter entre 3 e 80 caracteres.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O slug � obrigat�rio.")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "O slug deve conter entre 3 e 80 caracteres.")]
    public string Slug { get; set; }
}