using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PetIsland.Models.ViewModels;

public class ProductVM
{
    public ProductModel Product { get; set; }

    [Required(ErrorMessage = "Yêu cầu nhập bình luận sản phẩm")]
    public string Comment { get; set; }
    [Required(ErrorMessage = "Yêu cầu nhập tên")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Yêu cầu nhập email")]
    public string Email { get; set; }

    [ValidateNever]
    public IEnumerable<SelectListItem> CategoryList { get; set; }
}
