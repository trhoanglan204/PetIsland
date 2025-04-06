using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetIsland.DataAccess.Data;
using PetIsland.Models;
using PetIsland.Utility;

#pragma warning disable IDE0290

namespace PetIslandWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Route("Admin/PetCategory")]
//[Authorize(Roles = SD.Role_Admin)]
public class PetCatergoryController : Controller
{
    private readonly ApplicationDbContext _context;

    public PetCatergoryController(ApplicationDbContext context)
    {
        _context = context;
    }
    [Route("Index")]
    public async Task<IActionResult> Index()
    {
        var objCatagoryList = await _context.PetCategory.ToListAsync();

        return View(objCatagoryList);
    }

    [Route("Create")]
    public IActionResult Create()
    {
        return View();
    }

    [Route("Create")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PetCategoryModel category)
    {
        if (ModelState.IsValid)
        {
            category.Slug = category.Name.Replace(" ", "-");
            var slug = await _context.PetCategory.FirstOrDefaultAsync(p => p.Slug == category.Slug);
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
        PetCategoryModel? categoryFromDb = await _context.PetCategory.FindAsync(id);

        if (categoryFromDb == null)
        {
            return NotFound();
        }
        return View(categoryFromDb);
    }

    [Route("Edit")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PetCategoryModel category)
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

    public async Task<IActionResult> Delete(int? id)
    {
        PetCategoryModel? obj = await _context.PetCategory.FindAsync(id);
        if (obj == null)
        {
            return NotFound();
        }
        _context.PetCategory.Remove(obj);
        await _context.SaveChangesAsync();
        TempData["success"] = "Category Deleted Successfully";

        return RedirectToAction("Index");
    }
}
