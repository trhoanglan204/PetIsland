using System.ComponentModel.DataAnnotations;

namespace PetIsland.Models;

public class ProductQuantityModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Yêu cầu không được bỏ trống số lượng")]
    public required int Quantity { get; set; }

    public DateTime DateTime { get; set; }

    public long ProductId { get; set; }
}
