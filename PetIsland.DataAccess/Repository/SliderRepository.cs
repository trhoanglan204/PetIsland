using PetIsland.DataAccess.Data;
using PetIsland.DataAccess.Repository.IRepository;
using PetIsland.Models;

#pragma warning disable IDE0290

namespace PetIsland.DataAccess.Repository;

public class SliderRepository : Repository<SliderModel>, ISliderRepository
{
    private readonly ApplicationDbContext _context;
    public SliderRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    public Task UpdateAsync(SliderModel obj)
    {
        _context.Update(obj);
        return _context.SaveChangesAsync();
    }
}
