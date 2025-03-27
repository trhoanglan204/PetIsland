using System.ComponentModel.DataAnnotations;

namespace PetIsland.Models
{
    public class CouponModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập tên coupon")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập mô tả coupon")]
        public required string Description { get; set; }


        [Required(ErrorMessage = "Yêu cầu nhập ngày bắt đầu")]

        public required DateTime DateStart { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập ngày kết thúc")]


        public required DateTime DateExpired { get; set; }

        [Required(ErrorMessage = "Yêu cầu số lượng coupon")]
        public required int Quantity { get; set; }

        public int Status { get; set; }

    }
}
