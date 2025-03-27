using Microsoft.EntityFrameworkCore;
using PetIsland.DataAccess.Data;
using PetIsland.DataAccess.Repository.IRepository;
using PetIsland.Models;

#pragma warning disable IDE0290

namespace PetIsland.DataAccess.Repository;

public class WishlistRepository : Repository<WishlistModel>, IWishlistRepository
{
    private readonly ApplicationDbContext _context;
    public WishlistRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    public void Update(WishlistModel obj)
    {
        _context.Wishlist.Update(obj);
    }

    public async Task<object> GetWishlistModels()
    {
        var wishlist_product = await(from w in _context.Wishlist
                                     join p in _context.Products on w.ProductId equals p.Id
                                     select new { Product = p, Wishlists = w })
                           .ToListAsync();
        return wishlist_product;
    }
}
