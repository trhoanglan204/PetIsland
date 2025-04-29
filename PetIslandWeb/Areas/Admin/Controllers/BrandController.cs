using System.Drawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<BrandController> _logger;
    public BrandController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, ILogger<BrandController> logger)
    {
        _dataContext = context;
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
    }

    [HttpGet]
    [Route("Index")]
    public async Task<IActionResult> Index(int pg = 1)
    {
        var brand = await _dataContext.Brands.ToListAsync();

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
        ViewBag.Total = recsCount;

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
            if (brand.ImageUpload != null)
            {
                if (brand.ImageUpload.Length > 5 * 1024 * 1024) // Giới hạn 5MB
                {
                    ModelState.AddModelError("", "File ảnh không được lớn hơn 5MB.");
                    return View(brand);
                }
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/brands");
                if (!Directory.Exists(uploadsDir))
                {
                    Directory.CreateDirectory(uploadsDir);
                }
                string baseName = Path.GetFileNameWithoutExtension(brand.ImageUpload.FileName);
                if (string.IsNullOrEmpty(baseName))
                {
                    baseName = "brand_" + brand.Slug;
                }
                baseName = baseName.Length > 30 ? baseName[..30] : baseName;
                string imageName = baseName + "_" + Guid.NewGuid().ToString() + Path.GetExtension(brand.ImageUpload.FileName);
                string filePath = Path.Combine(uploadsDir, imageName);

                using var fs = new FileStream(filePath, FileMode.Create);
                await brand.ImageUpload.CopyToAsync(fs);
                fs.Close();
                brand.Image = imageName;
            }
            _dataContext.Brands.Add(brand);
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
            var existed_brand = _dataContext.Brands.Find(brand.Id); //tìm brand theo id brand
            if (existed_brand == null)
            {
                return NotFound();
            }
            brand.Slug = brand.Name.Replace(" ", "-");
            if (brand.ImageUpload != null)
            {
                if (brand.ImageUpload.Length > 5 * 1024 * 1024) // Giới hạn 5MB
                {
                    ModelState.AddModelError("", "File ảnh không được lớn hơn 5MB.");
                    return View(brand);
                }
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/brands");
                if (!Directory.Exists(uploadsDir))
                {
                    Directory.CreateDirectory(uploadsDir);
                }
                string baseName = Path.GetFileNameWithoutExtension(brand.ImageUpload.FileName);
                if (string.IsNullOrEmpty(baseName))
                {
                    baseName = "brand_" + brand.Slug;
                }
                baseName = baseName.Length > 30 ? baseName[..30] : baseName;
                string imageName = baseName + "_" + Guid.NewGuid().ToString() + Path.GetExtension(brand.ImageUpload.FileName);
                string filePath = Path.Combine(uploadsDir, imageName);

                using var fs = new FileStream(filePath, FileMode.Create);
                await brand.ImageUpload.CopyToAsync(fs);
                fs.Close();
                var oldImage = Path.Combine(uploadsDir, existed_brand.Image);
                if (System.IO.File.Exists(oldImage))
                {
                    try
                    {
                        System.IO.File.Delete(oldImage);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning("Không thể xóa file cũ {FilePath}: {Error}", oldImage, ex.Message);
                    }
                }
                existed_brand.Image = imageName;
            }
            existed_brand.Description = brand.Description;
            existed_brand.Status = brand.Status;
            existed_brand.Name = brand.Name;

            _dataContext.Update(existed_brand);
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
        string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/pets");
        string oldfilePath = Path.Combine(uploadsDir, brand.Image);
        if (System.IO.File.Exists(oldfilePath))
        {
            try
            {
                System.IO.File.Delete(oldfilePath);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Không thể xóa file cũ {FilePath}: {Error}", oldfilePath, ex.Message);
            }
        }
        _dataContext.Brands.Remove(brand);
        await _dataContext.SaveChangesAsync();
        TempData["success"] = "Thương hiệu đã được xóa thành công";
        return RedirectToAction("Index");
    }
}
