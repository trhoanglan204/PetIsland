namespace PetIsland.Models;

public class ProductDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public IFormFile? ImageFile { get; set; }
    public Initialize.Category Tags { get; set; } = Initialize.Category.Others;
    [Required]
    public double Price { get; set; }
    [Required]
    public string Description { get; set; } = string.Empty;
}
