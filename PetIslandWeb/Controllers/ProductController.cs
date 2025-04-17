using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using PetIsland.Models;
using PetIsland.DataAccess.Data;
using PetIsland.Models.ViewModels;
using System.Drawing.Drawing2D;

#pragma warning disable IDE0290

namespace PetIslandWeb.Controllers
{
	public class ProductController : Controller
	{
		private readonly ApplicationDbContext _dataContext;
		public ProductController(ApplicationDbContext context)
		{
			_dataContext = context;
		}
		public async Task<IActionResult> Index()
		{
			var products = await _dataContext.Products.ToListAsync();
			return View(products);
		}
		public async Task<IActionResult> Search(string searchTerm)
		{
			var products = await _dataContext.Products
			.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
			.ToListAsync();

			ViewBag.Keyword = searchTerm;

			return View(products);
		}

		public async Task<IActionResult> Detail(long? Id)
		{
			if (Id == null || Id <= 0) return RedirectToAction("Index");

			var productsById = _dataContext.Products.
				Include(p => p.Ratings).
				Where(p => p.Id == Id).FirstOrDefault()!; //category = 4
														 //related product

			var relatedProducts = await _dataContext.Products
			.Where(p => p.ProductCategoryId == productsById.ProductCategoryId && p.Id != productsById.Id)
			.Take(4)
			.ToListAsync();

			ViewBag.RelatedProducts = relatedProducts;

			var viewModel = new ProductVM
			{
				Product = productsById,
			};

			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]

		public async Task<IActionResult> CommentProduct(RatingModel rating)
		{
			if (ModelState.IsValid)
			{

				var ratingEntity = new RatingModel
				{
					ProductId = rating.ProductId,
					Name = rating.Name,
					Email = rating.Email,
					Comment = rating.Comment,
					Star = rating.Star

				};

				_dataContext.Ratings.Add(ratingEntity);
				await _dataContext.SaveChangesAsync();

				TempData["success"] = "Thêm đánh giá thành công";

				return Redirect(Request.Headers.Referer);
			}
			else
			{
				TempData["error"] = "Model có một vài thứ đang lỗi";
				var errors = new List<string>();
				foreach (var value in ModelState.Values)
				{
					foreach (var error in value.Errors)
					{
						errors.Add(error.ErrorMessage);
					}
				}
				//string errorMessage = string.Join("\n", errors);

				return RedirectToAction("Detail", new { id = rating.ProductId });
			}
		}
	}
}
