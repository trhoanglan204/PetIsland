using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetIsland.DataAccess.Data;
using PetIsland.Models;
using PetIsland.Utility;

#pragma warning disable IDE0290

namespace PetIslandWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Route("Admin/Brand")]
[Authorize(Roles = $"{SD.Role_Admin},{SD.Role_Employee}")]
public class BrandController : Controller
{
    private readonly ApplicationDbContext _dataContext;
    public BrandController(ApplicationDbContext context)
    {
        _dataContext = context;
    }

    //[Route("Index")]
    //public async Task<IActionResult> Index()
    //{
    //	return View(await _dataContext.Brands.OrderByDescending(p => p.Id).ToListAsync());
    //}

    [Route("Index")]
    public async Task<IActionResult> Index(int pg = 1)
    {
        List<BrandModel> brand = await _dataContext.Brands.ToListAsync();

        const int pageSize = 10;

        if (pg < 1)
        {
            pg = 1;
        }
        int recsCount = brand.Count;

        var pager = new Paginate(recsCount, pg, pageSize);

        int recSkip = (pg - 1) * pageSize;

        var data = brand.Skip(recSkip).Take(pager.PageSize).ToList();

        ViewBag.Pager = pager;

        return View(data);
    }

    [Route("Create")]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [Route("Create")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BrandModel brand)
    {
        if (ModelState.IsValid)
        {
            brand.Slug = brand.Name!.Replace(" ", "-");
            var slug = await _dataContext.Brands.FirstOrDefaultAsync(p => p.Slug == brand.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "Danh mục đã có trong database");
                return View(brand);
            }

            _dataContext.Add(brand);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Thêm thương hiệu thành công";
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
    [HttpGet]
    public async Task<IActionResult> Edit(int Id)
    {
        BrandModel? brand = await _dataContext.Brands.FindAsync(Id);
        if (brand == null)
        {
            return NotFound();
        }
        return View(brand);
    }

    [Route("Edit")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(BrandModel brand)
    {
        if (ModelState.IsValid)
        {
            brand.Slug = brand.Name!.Replace(" ", "-");
            _dataContext.Update(brand);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Cập nhật thương hiệu thành công";
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
    public async Task<IActionResult> Delete(int Id)
    {
        BrandModel? brand = await _dataContext.Brands.FindAsync(Id);
        if (brand == null)
        {
            return NotFound();
        }
        _dataContext.Brands.Remove(brand);
        await _dataContext.SaveChangesAsync();
        TempData["success"] = "Thương hiệu đã được xóa thành công";
        return RedirectToAction("Index");
    }
}
