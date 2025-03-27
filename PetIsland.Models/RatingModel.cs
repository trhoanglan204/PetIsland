using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetIsland.Models;

public class RatingModel
{
	[Key]
	public int Id { get; set; }

	public int ProductId { get; set; }
	[Required(ErrorMessage = "Yêu cầu nhập đánh giá sản phẩm")]
	public required string Comment { get; set; }
	[Required(ErrorMessage = "Yêu cầu nhập tên")]
	public required string Name { get; set; }
	[Required(ErrorMessage = "Yêu cầu nhập email")]
	public required string Email { get; set; }

	public string Star { get; set; }


	[ForeignKey("ProductId")]
	public ProductModel Product { get; set; }

}
