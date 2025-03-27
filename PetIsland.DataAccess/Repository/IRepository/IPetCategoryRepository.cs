using PetIsland.Models;

namespace PetIsland.DataAccess.Repository.IRepository;

public interface IPetCategoryRepository : IRepository<PetCategoryModel>
{
    void Update(PetCategoryModel obj);
}
