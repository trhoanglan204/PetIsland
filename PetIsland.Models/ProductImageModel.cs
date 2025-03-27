using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetIsland.Models;

public class ProductImageModel
{
    public int Id { get; set; }
    [Required]
    public string ImageUrl { get; set; } = string.Empty;
    public int ProductId { get; set; }
    [ForeignKey("ProductId")]
    public ProductModel? Product { get; set; }
}
