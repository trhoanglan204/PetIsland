using PetIsland.Models;

namespace PetIsland.DataAccess.Repository.IRepository;

public interface ISliderRepository : IRepository<SliderModel>
{
    Task UpdateAsync(SliderModel obj);
}
