using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using PetIsland.Models.Validation;

namespace PetIsland.Models;

public class SliderModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Yêu cầu không được bỏ trống tên slider")]
    public required string Name { get; set; }
    [Required(ErrorMessage = "Yêu cầu không được bỏ trống mô tả")]
    public required string Description { get; set; }
    public int? Status { get; set; }

    public string? Image { get; set; }

    [NotMapped]
    [FileExtension]
    public IFormFile? ImageUpload { get; set; }
}
