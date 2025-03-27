using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PetIsland.Models;

public class PetModel
{
    [Key]
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [Required]
    public Sexual Sex { get; set; } //0 Boy : 1 Girl  

    public int PetCategoryId { get; set; }
    [ForeignKey("PetCategoryId")]
    [ValidateNever]
    public PetCategoryModel PetCategory { get; set; }
    public DateTime Age { get; set; }
    public string Description { get; set; } = string.Empty;
    [ValidateNever]
    public List<PetImageModel>? PetImages { get; set; }

    [NotMapped]
    public IFormFile? ImageUrl { get; set; }
    [NotMapped]
    public string AgeDisplay
    {
        get
        {
            var now = DateTime.UtcNow;
            var age = now - Age;

            if (age.TotalDays < 365)
            {
                int months = (int)(age.TotalDays / 30);
                return $"{months} months";
            }
            else
            {
                int years = (int)(age.TotalDays / 365);
                return $"{years} years";
            }
        }
    }
    [NotMapped]
    public string SexDisplay => Sex == Sexual.Boy ? "Boy" : "Girl";
}

public enum Sexual : int 
{
Boy,
Girl,
}
