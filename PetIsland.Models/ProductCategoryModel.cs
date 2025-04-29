using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PetIsland.Models;

public class ProductCategoryModel
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Category's name must be filled in")]
    [MaxLength(30)]
    [DisplayName("Category Name")]
    public string Name { get; set; } = string.Empty;
    public string? Slug { get; set; }

}