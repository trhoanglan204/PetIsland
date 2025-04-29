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

            SeedRolesAndUsers();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while migrating the database.");
        }

    }
    private void CreateUserWithRole(AppUserModel user, string password, string role)
    {
        var result = _userManager.CreateAsync(user, password).GetAwaiter().GetResult();
        if (result.Succeeded)
        {
            _userManager.AddToRoleAsync(user, role).GetAwaiter().GetResult();
            _logger.LogInformation("Created user {Email} with role {Role}", user.Email, role);
        }
        else
        {
            foreach (var error in result.Errors)
            {
                _logger.LogError("Failed to create user {Email}: {Error}", user.Email, error.Description);
            }
        }
    }
    private void SeedRolesAndUsers() 
    {
        if (_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult()) return;

        foreach (var role in new[] { SD.Role_Customer, SD.Role_Employee, SD.Role_Admin })
        {
            _roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
        }
        CreateUserWithRole(new AppUserModel
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
            Avatar = "Admin.jpg"
        }, "Admin@123*", SD.Role_Admin);

        CreateUserWithRole(new AppUserModel
        {
            UserName = "vip_customer",
            Email = "customer@kma.com",
            Name = "AT19_Gang",
            PhoneNumber = "001122334455",
            StreetAddress = "Đầu đường xó chợ",
            State = "DK",
            PostalCode = "90001",
            City = "HaNoi",
            Role = SD.Role_Customer
        }, "Customer@123*", SD.Role_Customer);

        CreateUserWithRole(new AppUserModel
        {
            UserName = "staffA",
            Email = "staff@kma.com",
            Name = "AT19_Slave",
            PhoneNumber = "0987654321",
            StreetAddress = "Ut Tich",
            State = "DK",
            PostalCode = "90001",
            City = "HCM",
            Role = SD.Role_Employee,
            Avatar = "staffA.jpg"
        }, "Staff@123*", SD.Role_Employee);
    }
}
