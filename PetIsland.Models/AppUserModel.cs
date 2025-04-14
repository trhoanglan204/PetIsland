using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PetIsland.Models;

public class AppUserModel: IdentityUser
{
    public string Name { get; set; } = string.Empty;
    public string? StreetAddress { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Token { get; set; }
    public string Role { get; set; } = string.Empty;
    public string? Avatar { get; set; }
}
