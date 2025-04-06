using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetIsland.DataAccess.Data;
using PetIsland.Models;
using PetIsland.Utility;

#pragma warning disable IDE0290

namespace PetIslandWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Route("Admin/ProductCategory")]
//[Authorize(Roles = SD.Role_Admin)]
public class ProductCategoryController : Controller
{
    private readonly ApplicationDbContext _context;
    public ProductCategoryController(ApplicationDbContext context)
    {
        _context = context;
    }
    [Route("Index")]
    public async Task<IActionResult> Index(int pg = 1)
    {
        var objCatagoryList = await _context.ProductCategory.ToListAsync();

        const int pageSize = 10;
        if (pg < 1)
        {
            pg = 1;
        }

        int resCount = objCatagoryList.Count;
        var pager = new Paginate(resCount, pg, pageSize);
        int recSkip = (pg -1) * pageSize;

        var data = objCatagoryList.Skip(recSkip).Take(pager.PageSize).ToList();
        ViewBag.Pager = pager;

        return View(data);
    }

    [Route("Create")]
    public IActionResult Create()
    {
        return View();
    }

    [Route("Create")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCategoryModel category)
    {
        if (ModelState.IsValid)
        {
            category.Slug = category.Name.Replace(" ", "-");
            var slug = await _context.ProductCategory.FirstOrDefaultAsync(p => p.Slug == category.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "Danh mục đã có trong database");
                return View(category);
            }

            _context.Add(category);
            await _context.SaveChangesAsync();
            TempData["success"] = "Thêm danh mục thành công";
            return RedirectToAction("Index");

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
            string errorMessage = string.Join("\n", errors);
            return BadRequest(errorMessage);
        }
    }
    [Route("Edit")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        ProductCategoryModel? categoryFromDb = await _context.ProductCategory.FindAsync(id);

        if (categoryFromDb == null)
        {
            return NotFound();
        }
        return View(categoryFromDb);
    }
    [Route("Edit")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductCategoryModel category)
    {
        if (ModelState.IsValid)
        {
            category.Slug = category.Name.Replace(" ", "-");

            _context.Update(category);
            await _context.SaveChangesAsync();
            TempData["success"] = "Cập nhật danh mục thành công";
            return RedirectToAction("Index");

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
            string errorMessage = string.Join("\n", errors);
            return BadRequest(errorMessage);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        ProductCategoryModel? category = await _context.ProductCategory.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        _context.ProductCategory.Remove(category);
        await _context.SaveChangesAsync();
        TempData["success"] = "Danh mục đã được xóa thành công";
        return RedirectToAction("Index");
    }
}
