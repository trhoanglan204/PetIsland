using PetIsland.Utility;
using PetIsland.Models;
using PetIsland.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

#pragma warning disable IDE0290

namespace PetIsland.DataAccess.DbInitializer;

public class DbInitializer : IDbInitializer
{
    private readonly UserManager<AppUserModel> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DbInitializer> _logger;

    public DbInitializer(
        UserManager<AppUserModel> userManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context,
        ILogger<DbInitializer> logger)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _context = context;
        _logger = logger;
    }

    public void Initialize()
    {
        try
        {
            if (_context.Database.GetPendingMigrations().Any())
            {
                _context.Database.Migrate();
            }
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "An error occurred while migrating the database.");
        }

        //create roles if they are not created
        if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();

            //if roles are not created, then we will create admin user as well
            _userManager.CreateAsync(new AppUserModel
            {
                UserName = "admin@kma.com",
                Email = "admin@kma.com",
                Name = "AT19_MaThieuFamily",
                PhoneNumber = "0123456789",
                StreetAddress = "17A Cong Hoa",
                State = "DK",
                PostalCode = "90001",
                City = "HCM",
                Role = SD.Role_Admin,
            }, "Admin@123*").GetAwaiter().GetResult();

            AppUserModel user = _context.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@kma.com")!;
            _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
        }

        return;
    }
}
