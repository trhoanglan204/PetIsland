using PetIsland.DataAccess.Data;
using PetIsland.DataAccess.Repository.IRepository;
using PetIsland.Models;

#pragma warning disable IDE0290

namespace PetIsland.DataAccess.Repository;

public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
{
    private readonly ApplicationDbContext _context;
    public ShoppingCartRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(ShoppingCart obj)
    {
        _context.ShoppingCarts.Update(obj);
    }
}
