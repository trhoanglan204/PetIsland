namespace PetIsland.Models;

public class OrderModel
{
    public int Id { get; set; }
    public required string OrderCode { get; set; }
    public decimal ShippingCost { get; set; }
    public string? CouponCode { get; set; }
    public required string UserName { get; set; }
    public DateTime CreatedDate { get; set; }
    public int Status { get; set; }
}
