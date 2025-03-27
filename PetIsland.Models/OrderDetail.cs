using System.ComponentModel.DataAnnotations.Schema;

namespace PetIsland.Models;

public class OrderDetail
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string OrderCode { get; set; }
    public decimal Price { get; set; }

    public int Quantity { get; set; }
    public int ProductId { get; set; }


    [ForeignKey("ProductId")]
    public ProductModel Product { get; set; }
}
