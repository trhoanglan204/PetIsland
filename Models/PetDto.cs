namespace PetIsland.Models;

public class PetDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public IFormFile? ImageFile { get; set; }
    [Required]
    public Initialize.Sexual Sex { get; set; } //0 Boy : 1 Girl  
    public string Color { get; set; } = string.Empty;
    [Required]
    public double Age { get; set; }
    [Required]
    public Initialize.TypePet Tags { get; set; }
    public string Description { get; set; } = string.Empty;
}
