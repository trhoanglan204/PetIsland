using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PetIsland.Models.Momo;

public class MomoInfoModel
{
    [Key]
    public int Id { get; set; }
    public string? OrderId { get; set; }
    public string? OrderInfo { get; set; }
    public string? FullName { get; set; }
    [Precision(18, 2)]
    public decimal Amount { get; set; }
    public DateTime DatePaid { get; set; }
}
