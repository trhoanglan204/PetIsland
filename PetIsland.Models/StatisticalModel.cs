using System.ComponentModel.DataAnnotations;

#pragma warning disable IDE1006

namespace PetIsland.Models;

public class StatisticalModel
{
    [Key]
    public int Id { get; set; }
    public decimal revenue { get; set; }
    public int orders { get; set; }
    public DateTime date { get; set; }

}
