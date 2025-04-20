using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using PetIsland.DataAccess.Data;
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
		public IActionResult Index()
		{
			return View();
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
