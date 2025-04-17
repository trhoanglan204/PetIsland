using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PetIsland.Models.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetIsland.Models;
public partial class ProductModel
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Product's name should be filled in")]
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public int SoldOut { get; set; } = 0;
    [Range(1, 99999)]
    public int Quantity { get; set; } = 0;
    public string Slug { get; set; } = string.Empty;
    public string Image { get; set; } = "null.jpg";
    public RatingModel? Ratings { get; set; }
    public int? BrandId { get; set; }
    public BrandModel? Brand { get; set; }
    public int ProductCategoryId { get; set; }
    [ForeignKey("ProductCategoryId")]
    [ValidateNever]
    public ProductCategoryModel ProductCategory { get; set; }

    [Required(ErrorMessage = "Product's price should be filled in")]
    [Range(1, double.MaxValue, ErrorMessage = "Price should be greater than $0.01")]
    public decimal Price { get; set; } = 0;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [NotMapped]
    [FileExtension]
    public IFormFile? ImageUpload { get; set; }
}

