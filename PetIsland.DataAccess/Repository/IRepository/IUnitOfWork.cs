namespace PetIsland.DataAccess.Repository.IRepository;

public interface IUnitOfWork
{
    IProductCategoryRepository ProductCategory { get; }
    IPetCategoryRepository PetCategory { get; }
    IProductRepository Product { get; }
    IPetRepository Pet { get; }
    IAppUserRepository ApplicationUser { get; }
    IProductImageRepository ProductImage { get; }
    IShoppingCartRepository ShoppingCart { get; }
    ISliderRepository Slider { get; }
    IWishlistRepository Wishlist { get; }

    Task SaveAsync();
}
