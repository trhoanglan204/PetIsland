using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PetIsland.Models;

public class OrderDetail
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? OrderCode { get; set; }
    [Precision(18,2)]
    public decimal Price { get; set; }

    public int Quantity { get; set; }
    public long ProductId { get; set; }


    [ForeignKey("ProductId")]
    public ProductModel? Product { get; set; }
}
