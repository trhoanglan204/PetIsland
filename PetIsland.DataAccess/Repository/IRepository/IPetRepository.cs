using PetIsland.Models;

namespace PetIsland.DataAccess.Repository.IRepository;

public interface IPetRepository : IRepository<PetModel>
{
    void Update(PetModel obj);
}
