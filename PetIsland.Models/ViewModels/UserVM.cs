using PetIsland.Models.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace PetIsland.Models.ViewModels;

public class UserVM
{
    [Required(ErrorMessage = "Vui lòng nhập user name")]
    [RegularExpression(@"^\S+$", ErrorMessage = "Username không được chứa khoảng trắng")]
    public string? Username { get; set; } 
    [Required(ErrorMessage = "Vui lòng nhập user email"), EmailAddress]
    public string? Email { get; set; }
    [DataType(DataType.Password), Required(ErrorMessage = "Vui lòng nhập password")]
    public required string Password { get; set; }

    public string Image { get; set; } = "blank_avatar.jpg"; //default
    public string Name { get; set; } = string.Empty;
    public string? StreetAddress { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }

    [NotMapped]
    [FileExtension]
    public IFormFile? ImageUpload { get; set; }
}
//nullable due to may login as Google