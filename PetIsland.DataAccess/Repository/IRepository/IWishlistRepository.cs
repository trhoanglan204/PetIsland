using PetIsland.Models;

namespace PetIsland.DataAccess.Repository.IRepository;

public interface IWishlistRepository : IRepository<WishlistModel>
{
    void Update(WishlistModel obj);
    Task<object> GetWishlistModels();
}
