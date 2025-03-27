using PetIsland.Models;
using PetIsland.DataAccess.Repository.IRepository;
using PetIsland.DataAccess.Data;

#pragma warning disable IDE0290

namespace PetIsland.DataAccess.Repository;

public class ProductRepository : Repository<ProductModel>, IProductRepository
{
    private readonly ApplicationDbContext _context;
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public Task UpdateAsync(ProductModel obj)
    {
        var objFromDb = _context.Products.FirstOrDefault(u => u.Id == obj.Id);
        if (objFromDb != null)
        {
            objFromDb.OldPrice = objFromDb.Price;
            objFromDb.Price = obj.Price;
            objFromDb.Description = obj.Description;
            objFromDb.ProductCategoryId = obj.ProductCategoryId;
            objFromDb.ProductImages = obj.ProductImages;
            objFromDb.Name = obj.Name;
            objFromDb.CreatedDate = obj.CreatedDate;
        }
        return _context.SaveChangesAsync();
    }
}