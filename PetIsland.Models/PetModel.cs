using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PetIsland.Models.Validation;

namespace PetIsland.Models;

public class PetModel
{
    [Key]
    public long Id { get; set; }
    [Required, MaxLength(100)]
    public required string Name { get; set; }
    [Required(ErrorMessage = "Vui lòng chọn giới tính cho thú cưng")]
    public Sexual? Sex { get; set; } //0 Boy : 1 Girl  
    public string Slug { get; set; } = string.Empty;
    public string Image { get; set; } = "null.jpg";
    public int PetCategoryId { get; set; }
    [ForeignKey("PetCategoryId")]
    [ValidateNever]
    public PetCategoryModel? PetCategory { get; set; }

    public DateTime Age { get; set; }
    public string Description { get; set; } = string.Empty;

    [NotMapped]
    [FileExtension]
    public IFormFile? ImageUpload { get; set; }
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