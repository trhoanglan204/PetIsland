using System.ComponentModel.DataAnnotations;

namespace PetIsland.Models.ViewModels;

public class ResetPasswordVM
{
    [Required]
    [DataType(DataType.Password)]
    public string? OldPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "Password do not match.")]
    public string? ConfirmPassword { get; set; }
}
