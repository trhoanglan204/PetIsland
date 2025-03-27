using PetIsland.Models;
using PetIsland.DataAccess.Repository.IRepository;
using PetIsland.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

#pragma warning disable IDE0290

namespace PetIsland.DataAccess.Repository;

public class AppUserRepository : Repository<AppUserModel>, IAppUserRepository
{
    private readonly ApplicationDbContext _context;
    public AppUserRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    public void Update(AppUserModel applicationUser)
    {
        _context.ApplicationUsers.Update(applicationUser);
    }

    public async Task<object> GetUsersWithRoles()
    {
        var usersWithRoles = await (from u in _context.Users
                                    join ur in _context.UserRoles on u.Id equals ur.UserId
                                    join r in _context.Roles on ur.RoleId equals r.Id
                                    select new { User = u, RoleName = r.Name }).ToListAsync();
        return usersWithRoles;
    }
}
 
