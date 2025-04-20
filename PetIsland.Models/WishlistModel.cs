using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PetIsland.Models
{
    public class WishlistModel
    {
        [Key]
        public int Id { get; set; }
        public long ProductId { get; set; }
        public required string UserId { get; set; }

        [ForeignKey("ProductId")]
        public ProductModel? Product { get; set; }
    }
}
