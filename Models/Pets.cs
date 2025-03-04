namespace PetIsland.Models;

public class Pets
{
    [Key]
    public int PetId { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public Initialize.Sexual? Sex { get; set; } //0 Boy : 1 Girl  
    public string? Color { get; set; } = string.Empty;
    public int? Age { get; set; }
    public string? Description { get; set; } = string.Empty;
    public Initialize.TypePet Tags { get; set; }

    public Pets() //Constructor
    {
    }
}


