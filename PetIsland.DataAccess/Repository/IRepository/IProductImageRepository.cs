using PetIsland.Models;    

namespace PetIsland.DataAccess.Repository.IRepository;

public interface IProductImageRepository : IRepository<ProductImageModel>
{
    Task UpdateAsync(ProductImageModel obj);
}
