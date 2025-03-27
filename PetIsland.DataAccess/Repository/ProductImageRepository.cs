
using PetIsland.Models;
using PetIsland.DataAccess.Repository.IRepository;
using PetIsland.DataAccess.Data;

#pragma warning disable IDE0290

namespace PetIsland.DataAccess.Repository;

public class ProductImageRepository : Repository<ProductImageModel>, IProductImageRepository
{
    private ApplicationDbContext _context;
    public ProductImageRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public Task UpdateAsync(ProductImageModel obj)
    {
        _context.ProductImages.Update(obj);
        return _context.SaveChangesAsync();
    }
}
