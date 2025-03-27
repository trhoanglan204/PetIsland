using PetIsland.Models;
using PetIsland.DataAccess.Repository.IRepository;
using PetIsland.DataAccess.Data;

#pragma warning disable IDE0290

namespace PetIsland.DataAccess.Repository;

public class ProductCategoryRepository : Repository<ProductCategoryModel>, IProductCategoryRepository
{
    private readonly ApplicationDbContext _context;
    public ProductCategoryRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(ProductCategoryModel obj)
    {
        _context.ProductCategory.Update(obj);
    }
}
