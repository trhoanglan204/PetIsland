using PetIsland.Models;
using PetIsland.DataAccess.Repository.IRepository;
using PetIsland.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

#pragma warning disable IDE0290

namespace PetIsland.DataAccess.Repository;

public class PetRepository : Repository<PetModel>, IPetRepository
{
    private readonly ApplicationDbContext _context;
    public PetRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    public void Update(PetModel obj)
    {
        var objFromDb = _context.Pets.FirstOrDefault(u => u.Id == obj.Id);
        if (objFromDb != null)
        {
            objFromDb.Name = objFromDb.Name;
            objFromDb.Description = obj.Description;
            objFromDb.PetCategoryId = obj.PetCategoryId;
            objFromDb.PetImages = obj.PetImages;
            objFromDb.Age = obj.Age;
            objFromDb.Sex = obj.Sex;
        }
    }
}