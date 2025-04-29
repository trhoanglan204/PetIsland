using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using PetIsland.Models;
using PetIsland.DataAccess.Data;
using PetIsland.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

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
		public async Task<IActionResult> Index(int pg = 1)
		{
			var products = await _dataContext.Products.ToListAsync();

            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }

            int resCount = products.Count;
            var pager = new Paginate(resCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;

            var data = products.Skip(recSkip).Take(pager.PageSize).ToList();
            ViewBag.Pager = pager;
			ViewBag.Total = resCount;

            return View(data);
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

			var productsById = await _dataContext.Products.
				Include(p => p.Ratings).
				Include(p => p.ProductCategory).
                Include(p => p.Brand).
                FirstOrDefaultAsync(p => p.Id == Id);
			if (productsById == null)
			{
				return NotFound();
			}											
			var relatedProducts = await _dataContext.Products
			.Where(p => p.ProductCategoryId == productsById.ProductCategoryId && p.Id != productsById.Id)
			.Take(4)
			.ToListAsync();

            ViewBag.RelatedProducts = relatedProducts;

            var currentUserEmail = User.Identity?.Name ?? "";

            var topPositive = await _dataContext.RatingEntries
				.Where(r => r.ProductId == Id && r.Email != currentUserEmail && r.Star >= 3)
				.OrderByDescending(r => r.Star)
                .ThenByDescending(r => r.Id)
                .Take(5)
				.ToListAsync();

            var topNegative = await _dataContext.RatingEntries
                .Where(r => r.ProductId == Id && r.Email != currentUserEmail && r.Star < 3)
                .OrderBy(r => r.Star)
                .ThenBy(r => r.Id)
                .Take(5)
                .ToListAsync();

			var allRating = await _dataContext.RatingEntries.Where(r => r.ProductId == Id && r.Email != currentUserEmail).ToListAsync();
			ViewBag.AllRatingExceptUser = allRating;

            ViewBag.TopPositiveRatings = topPositive;
			ViewBag.TopNegativeRatings = topNegative;

			var userRating = await _dataContext.RatingEntries.FirstOrDefaultAsync(r => r.ProductId == Id && r.Email == currentUserEmail);
			ViewBag.UserRatingEntry = userRating;

            var viewModel = new ProductVM
			{
				Product = productsById,
			};

			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> CommentProduct(ProductVM rating, string? returnUrl)
		{
			if (ModelState.IsValid)
			{
				var productId = rating.Product!.Id;
				var userEmail = User.Identity!.Name!;

                var ratingEntry = await _dataContext.RatingEntries.FirstOrDefaultAsync(r => r.ProductId == productId && r.Email == userEmail);

				if (ratingEntry == null)
				{
					//user rate for first time
					ratingEntry = new RatingEntryModel 
					{ 
						ProductId = productId,
						Email = userEmail,
						Star = rating.UserStar,
						RatingDate = DateTime.UtcNow,
						Comment = rating.Comment
					};
					await _dataContext.RatingEntries.AddAsync(ratingEntry);
					var ratingModel = await _dataContext.Ratings.FirstOrDefaultAsync(r => r.ProductId == productId);
                    if (ratingModel != null)
                    {
                        ratingModel.TotalRated += 1;
                    }
                }
				else
				{
					//user edit their rating
					ratingEntry.Star = rating.UserStar;
					ratingEntry.Comment = rating.Comment;
					ratingEntry.RatingDate = DateTime.UtcNow;
				}
				await _dataContext.SaveChangesAsync();

                // update trung bình
                var ratings = _dataContext.RatingEntries.Where(r => r.ProductId == productId);
                var avg = await ratings.AverageAsync(r => r.Star);
                var existingRating = await _dataContext.Ratings.FirstOrDefaultAsync(r => r.ProductId == productId);
                if (existingRating != null)
                {
                    existingRating.Star = Math.Round((decimal)avg, 1);
                    await _dataContext.SaveChangesAsync();
                }
                TempData["success"] = "Thêm đánh giá thành công";

                //return Redirect(Request.Headers.Referer);
                return Redirect(returnUrl ?? Url.Action("Detail", new { id = productId })!);
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
				return RedirectToAction("Detail", new { id = rating.Product!.Id });
			}
		}
	}
}
