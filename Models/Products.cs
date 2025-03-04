namespace PetIsland.Models;
public class Products
{
    [Key]
    public int Id { get; set; }

    [Required,MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public Initialize.Category Tags { get; set; }

    [Precision(10, 2)]
    public double Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public Products() //Constructor
    {
    }
}

