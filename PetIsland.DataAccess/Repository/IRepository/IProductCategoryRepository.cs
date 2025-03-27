using PetIsland.Models;

namespace PetIsland.DataAccess.Repository.IRepository;

public interface IProductCategoryRepository : IRepository<ProductCategoryModel>
{
    void Update(ProductCategoryModel obj); 
}
