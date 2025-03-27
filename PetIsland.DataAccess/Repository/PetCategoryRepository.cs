using PetIsland.Models;
using PetIsland.DataAccess.Repository.IRepository;
using PetIsland.DataAccess.Data;

#pragma warning disable IDE0290

namespace PetIsland.DataAccess.Repository;

public class PetCategoryRepository : Repository<PetCategoryModel>, IPetCategoryRepository
{
    private readonly ApplicationDbContext _context;
    public PetCategoryRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    public void Update(PetCategoryModel obj)
    {
        _context.PetCategory.Update(obj);
    }
}