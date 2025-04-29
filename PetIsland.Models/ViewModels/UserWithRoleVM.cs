using Microsoft.AspNetCore.Identity;

namespace PetIsland.Models.ViewModels;

public class UserWithRoleVM
{
    public IdentityUser? User { get; set; }
    public string? RoleName { get; set; }
}
