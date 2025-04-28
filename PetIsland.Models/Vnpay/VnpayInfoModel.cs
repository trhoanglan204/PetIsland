using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetIsland.Models.Vnpay;

public class VnpayInfoModel
{
    [Key]
    public int Id { get; set; }
    public string? OrderId { get; set; }
    public string? OrderDescription { get; set; }
    public string? TransactionId { get; set; }
    public string? PaymentId { get; set; }
    public string? PaymentMethod { get; set; }
    public DateTime DatePaid { get; set; }
}
