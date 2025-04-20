using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PetIsland.Models.ViewModels;

public class PetVM
{
    public PetModel? Pet { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem>? CategoryList { get; set; }
}
