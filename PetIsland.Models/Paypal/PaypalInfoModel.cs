using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

#pragma warning disable IDE1006

namespace PetIsland.Models.Paypal;

public class PaypalInfoModel
{
    [Key]
    public int Id { get; set; }
    public string? status { get; set; }
    public string? OrderId { get; set; }
    public string? OrderInfo { get; set; }
    public string? FullName { get; set; }
    [Precision(18, 2)]
    public decimal Amount { get; set; }
    public DateTime DatePaid { get; set; }
}
