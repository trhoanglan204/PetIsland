using System.Drawing.Drawing2D;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using PetIsland.DataAccess.Data;
using PetIsland.Models;
using PetIsland.Models.ViewModels;

#pragma warning disable IDE0290

namespace PetIslandWeb.Controllers
{
	public class PetController : Controller
	{
		private readonly ApplicationDbContext _dataContext;
		public PetController(ApplicationDbContext context)
		{
			_dataContext = context;
		}
		public async Task<IActionResult> Index(int pg = 1)
		{
            var pets = await _dataContext.Pets
                .Include(p => p.PetCategory)
                .ToListAsync();

            const int pageSize = 10;

            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = pets.Count;

            var pager = new Paginate(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;

            var data = pets.Skip(recSkip).Take(pager.PageSize).ToList();

            ViewBag.Pager = pager;
			ViewBag.Total = recsCount;

            return View(data);
		}
		public async Task<IActionResult> Search(string searchTerm)
		{
			var pets = await _dataContext.Pets
			.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
			.ToListAsync();

			ViewBag.Keyword = searchTerm;

			return View(pets);
		}

		public async Task<IActionResult> Detail(long? Id)
		{
			if (Id == null || Id <= 0) return RedirectToAction("Index");

			var petsById = _dataContext.Pets.Where(p => p.Id == Id).FirstOrDefault()!; 

			var relatedPets = await _dataContext.Pets
            .Where(p => p.PetCategoryId == petsById.PetCategoryId && p.Id != petsById.Id)
			.Take(4)
			.ToListAsync();

			ViewBag.RelatedPets = relatedPets;

			var viewModel = new PetVM
			{
				Pet = petsById,
			};

			return View(viewModel);
		}
    }
}
