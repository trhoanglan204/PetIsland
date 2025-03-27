using Microsoft.AspNetCore.Mvc.Rendering;

namespace PetIsland.Models.ViewModels;

public class RoleManagmentVM
{
    public AppUserModel? ApplicationUser { get; set; }
    public IEnumerable<SelectListItem>? RoleList { get; set; }
}
