using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetIsland.Models;

public class PetImageModel
{
    public int Id { get; set; }
    [Required]
    public string ImageUrl { get; set; } = string.Empty;
    public int PetId { get; set; }
    [ForeignKey("PetId")]
    public PetModel Pet { get; set; }
}
