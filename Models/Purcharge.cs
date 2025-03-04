namespace PetIsland.Models;
public class CartItem
{
    public required Products Product { get; set; }
    public int Quantity { get; set; }
    public double TotalPrice => Product.Price * Quantity;
}

public class Cart
{
    public int Id { get; set; }
    public List<CartItem> Items { get; set; } = [];
    public int TotalQuantity => Items.Sum(item => item.Quantity);
    public double TotalPrice => Items.Sum(x => x.TotalPrice);
}

public class Orders
{
    public int Id { get; set; }
    public required Cart Items { get; set; }
    public Initialize.State Status { get; set; } = Initialize.State.Pending;
    public Initialize.PaymentMethod Payment { get; set; }
    public DateTime TimeOrder { get; set; } = DateTime.UtcNow;
    public bool IsPaid { get; set; } = false;
    public bool IsDelivered { get; set; } = false;
    public double TotalPrice => Items.TotalPrice;
    public bool IsRefunded => Status == Initialize.State.Refunded;
}

