using PetIsland.Models;

namespace PetIsland.DataAccess.Repository.IRepository;

public interface IProductRepository : IRepository<ProductModel>
{
    Task UpdateAsync(ProductModel obj); 
}
