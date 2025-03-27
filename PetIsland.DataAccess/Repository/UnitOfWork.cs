using PetIsland.DataAccess.Repository.IRepository;
using PetIsland.DataAccess.Data;

namespace PetIsland.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IProductCategoryRepository ProductCategory { get; private set; }
    public IPetCategoryRepository PetCategory { get; private set; }
    public IProductRepository Product { get; private set; }
    public IPetRepository Pet { get; private set; }
    public IAppUserRepository ApplicationUser { get; private set; }
    public IProductImageRepository ProductImage { get; private set; }
    public IShoppingCartRepository ShoppingCart { get; private set; }
    public ISliderRepository Slider { get; private set; }
    public IWishlistRepository Wishlist { get; private set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        ProductImage = new ProductImageRepository(_context);
        ProductCategory = new ProductCategoryRepository(_context);
        Product = new ProductRepository(_context);
        PetCategory = new PetCategoryRepository(_context);
        Pet = new PetRepository(_context);
        ApplicationUser = new AppUserRepository(_context);
        ShoppingCart = new ShoppingCartRepository(_context);
        Slider = new SliderRepository(_context);
        Wishlist = new WishlistRepository(_context);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
