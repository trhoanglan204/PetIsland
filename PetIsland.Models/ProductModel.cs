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
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int SoldOut { get; set; }
    public int Quantity { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Image { get; set; }

    public RatingModel Ratings { get; set; }
    public int ProductCategoryId { get; set; }
    [ForeignKey("ProductCategoryId")]
    [ValidateNever]
    public ProductCategoryModel ProductCategory { get; set; }
    [NotMapped]
    private decimal _oldPrice;
    [Range(1, double.MaxValue, ErrorMessage = "Old Price should be less than current Price")]
    public decimal OldPrice
    {
        get => _oldPrice;
        set
        {
            if (value < Price)
            {
                _oldPrice = value;
            }
            else
            {
                _oldPrice = Price;
            }
        }
    }

    [Required(ErrorMessage = "Product's price should be filled in")]
    [Range(1, double.MaxValue, ErrorMessage = "Price should be greater than $0.01")]
    public decimal Price { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [ValidateNever]
    public List<ProductImageModel>? ProductImages { get; set; }

    [NotMapped]
    [FileExtension]
    public IFormFile? ImageUpload { get; set; }
}

