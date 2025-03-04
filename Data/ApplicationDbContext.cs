namespace PetIsland.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Products> Products { get; set; }
    public DbSet<Pets> Pets { get; set; }
}
