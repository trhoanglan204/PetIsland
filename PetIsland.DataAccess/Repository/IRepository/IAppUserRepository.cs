using PetIsland.Models;

namespace PetIsland.DataAccess.Repository.IRepository;

public interface IAppUserRepository : IRepository<AppUserModel>
{
    public void Update(AppUserModel applicationUser);
    public Task<object> GetUsersWithRoles();
}
