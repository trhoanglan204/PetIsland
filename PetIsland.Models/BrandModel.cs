using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using PetIsland.Models.Validation;

namespace PetIsland.Models;

public class BrandModel
{
	public int Id { get; set; }
	[Required(ErrorMessage = "Yêu cầu không được bỏ trống tên thương hiệu")]
	public required string Name { get; set; }
	[Required(ErrorMessage = "Yêu cầu không được bỏ trống mô tả")]
	public string? Description { get; set; }
	public string? Slug { get; set; }
	public int? Status { get; set; }

	public string Image { get; set; } = "null.jpg";

	[NotMapped]
	[FileExtension]
	public IFormFile? ImageUpload { get; set; }
}
