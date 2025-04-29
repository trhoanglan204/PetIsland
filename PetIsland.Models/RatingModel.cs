using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PetIsland.Models;

public class RatingModel
{
	[Key]
	public int Id { get; set; }
	[Required(ErrorMessage = "Yêu cầu nhập đánh giá sản phẩm")]
	public long TotalRated { get; set; }
	[Range(1,5)]
	[Precision(3,1)]
    public decimal Star { get; set; }
    public long ProductId { get; set; }

    [ForeignKey("ProductId")]
	public ProductModel Product { get; set; }
}
