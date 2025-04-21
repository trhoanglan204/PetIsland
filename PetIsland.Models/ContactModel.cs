using Microsoft.AspNetCore.Http;
using PetIsland.Models.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetIsland.Models;

public class ContactModel
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Yêu cầu nhập tiêu đề website")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Yêu cầu nhập địa chỉ")]
    public string? Address { get; set; }

    [Required(ErrorMessage = "Yêu cầu nhập Email")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Yêu cầu nhập SDT")]
    public string? Phone { get; set; }

    public string? ORS_Key { get; set; }

    public double ORS_lon { get; set; }
    public double ORS_lat { get; set; }
}
