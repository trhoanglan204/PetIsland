using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PetIsland.Models;

public class ShippingModel
{
    public int Id { get; set; }
    [Precision(18,2)]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Yêu cầu chọn tên xã phường")]
    public required string Ward { get; set; }
    [Required(ErrorMessage = "Yêu cầu chọn quận huyện")]
    public required string District { get; set; }
    [Required(ErrorMessage = "Yêu cầu chọn thành phố")]
    public required string City { get; set; }
}
