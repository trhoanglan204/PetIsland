using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetIsland.Models;
using PetIsland.DataAccess.Data;

#pragma warning disable IDE0290

namespace PetIslandWeb.Controllers;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext _dataContext;
    public CategoryController(ApplicationDbContext context)
    {
        _dataContext = context;
    }
    public async Task<IActionResult> Index(string slug = "", string sort_by = "", string startprice = "", string endprice = "")
    {
        ProductCategoryModel? category = _dataContext.ProductCategory.Where(c => c.Slug == slug).FirstOrDefault();

        if (category == null)
        {
            return RedirectToAction("Index");
        }

        ViewBag.Slug = slug;
        IQueryable<ProductModel> productsByCategory = _dataContext.Products.Where(p => p.ProductCategoryId == category.Id);
        var count = await productsByCategory.CountAsync();
        if (count > 0)
        {
            if (sort_by == "price_increase")
            {
                productsByCategory = productsByCategory.OrderBy(p => p.Price);
            }
            else if (sort_by == "price_decrease")
            {
                productsByCategory = productsByCategory.OrderByDescending(p => p.Price);
            }
            else if (sort_by == "price_newest")
            {
                productsByCategory = productsByCategory.OrderByDescending(p => p.Id);
            }
            else if (sort_by == "price_oldest")
            {
                productsByCategory = productsByCategory.OrderBy(p => p.Id);
            }
            else if (startprice != "" && endprice != "")
            {

                if (decimal.TryParse(startprice, out decimal startPriceValue) && decimal.TryParse(endprice, out decimal endPriceValue))
                {
                    productsByCategory = productsByCategory.Where(p => p.Price >= startPriceValue && p.Price <= endPriceValue);
                }
                else
                {
                    productsByCategory = productsByCategory.OrderByDescending(p => p.Id);
                }
            }
            else
            {
                productsByCategory = productsByCategory.OrderByDescending(p => p.Id);
            }

            decimal minPrice = await productsByCategory.MinAsync(p => p.Price);
            decimal maxPrice = await productsByCategory.MaxAsync(p => p.Price);


            ViewBag.sort_key = sort_by;

            ViewBag.count = count;

            ViewBag.minprice = minPrice;
            ViewBag.maxprice = maxPrice;
        }

        // Add more sorting options if needed (e.g., name, date)
        return View(await productsByCategory.ToListAsync());
    }
}
