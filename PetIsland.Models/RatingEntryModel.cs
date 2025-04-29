using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PetIsland.Models;

public class RatingEntryModel
{
    [Key]
    public int Id { get; set; }

    public long ProductId { get; set; }

    [ForeignKey("ProductId")]
    public ProductModel Product { get; set; }

    [Required]
    public required string Email { get; set; }

    public string? Comment { get; set; }
    public DateTime RatingDate { get; set; }

    [Range(1, 5)]
    public int Star { get; set; }
}
