using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace PetIsland.Models;

public class UserModel
{

	public int Id { get; set; }
	[Required(ErrorMessage = "Vui lòng nhập user name")]
	public string Username { get; set; }
	[Required(ErrorMessage = "Vui lòng nhập user email"), EmailAddress]
	public string Email { get; set; }
	[DataType(DataType.Password), Required(ErrorMessage = "Vui lòng nhập password")]
	public string Password { get; set; }
	public string Image = "blank_avatar.jpg";

	[NotMapped]
	[FileExtensions]
	public IFormFile? ImageUpload { get; set; }
}
