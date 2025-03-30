using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetIsland.Models;
namespace PetIsland.DataAccess.Data;

#pragma warning disable IDE0290

public class ApplicationDbContext : IdentityDbContext<AppUserModel>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
    public DbSet<PetModel> Pets { get; set; }
    public DbSet<PetCategoryModel> PetCategory { get; set; }
    public DbSet<ProductModel> Products { get; set; }
    public DbSet<ProductCategoryModel> ProductCategory { get; set; }
    public DbSet<AppUserModel> ApplicationUsers { get; set; }
    public DbSet<ProductImageModel>  ProductImages { get; set; }
    public DbSet<PetImageModel>  PetImages { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    //public DbSet<GroupMemberModel> Members { get; set; }
    public DbSet<OrderModel> Orders { get; set; }
    public DbSet<ShippingModel> Shippings { get; set; }
    public DbSet<CouponModel> Coupons { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<SliderModel> Sliders { get; set; }
    public DbSet<WishlistModel> Wishlist { get; set; }
    public DbSet<RatingModel> Ratings { get; set; }
}
