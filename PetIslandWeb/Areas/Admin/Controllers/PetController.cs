using PetIsland.Models;
using PetIsland.Models.ViewModels;
using PetIsland.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PetIsland.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

#pragma warning disable IDE0290

namespace PetIslandWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Route("Admin/Pet")]
[Authorize(Roles = $"{SD.Role_Admin},{SD.Role_Employee}")]
public class PetController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<PetController> _logger;
    public PetController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, ILogger<PetController> logger)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
    }

    [HttpGet]
    [Route("Index")]
    public async Task<IActionResult> Index(int pg = 1)
    {
        var objCatagoryList = await _context.Pets.OrderByDescending(p => p.Id).Include(c => c.PetCategory).ToListAsync();
        const int pageSize = 10;
        if (pg < 1)
        {
            pg = 1;
        }

        int resCount = objCatagoryList.Count;
        var pager = new Paginate(resCount, pg, pageSize);
        int recSkip = (pg - 1) * pageSize;

        var data = objCatagoryList.Skip(recSkip).Take(pager.PageSize).ToList();
        ViewBag.Pager = pager;
        ViewBag.Total = resCount;

        return View(data);
    }

    [HttpGet]
    [Route("Create")]
    public IActionResult Create()
    {
        ViewBag.Categories = new SelectList(_context.PetCategory, "Id", "Name");
        return View();
    }

    [Route("Create")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PetModel pet)
    {
        ViewBag.Categories = new SelectList(_context.PetCategory, "Id", "Name", pet.PetCategoryId);

        if (ModelState.IsValid)
        {
            pet.Slug = pet.Name.Replace(" ", "-");
            var slug = await _context.Products.FirstOrDefaultAsync(p => p.Slug == pet.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "Thú cưng đã có trong database");
                return View(pet);
            }

            if (pet.ImageUpload != null)
            {
                if (pet.ImageUpload.Length > 5 * 1024 * 1024) // Giới hạn 5MB
                {
                    ModelState.AddModelError("", "File ảnh không được lớn hơn 5MB.");
                    return View(pet);
                }
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/pets");
                if (!Directory.Exists(uploadsDir))
                {
                    Directory.CreateDirectory(uploadsDir);
                }
                string baseName = Path.GetFileNameWithoutExtension(pet.ImageUpload.FileName);
                if (string.IsNullOrEmpty(baseName))
                {
                    baseName = "pet_" + pet.Slug;
                }
                baseName = baseName.Length > 30 ? baseName[..30] : baseName;
                string imageName = baseName + "_" + Guid.NewGuid().ToString() + Path.GetExtension(pet.ImageUpload.FileName);
                string filePath = Path.Combine(uploadsDir, imageName);

                using var fs = new FileStream(filePath, FileMode.Create);
                await pet.ImageUpload.CopyToAsync(fs);
                fs.Close();
                pet.Image = imageName;
            }
            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();
            TempData["success"] = "Thêm thú cưng thành công";
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
    public async Task<IActionResult> Edit(long Id)
    {
        PetModel? pet = await _context.Pets.FindAsync(Id);
        if (pet == null)
        {
            return NotFound();
        }
        ViewBag.Categories = new SelectList(_context.PetCategory, "Id", "Name", pet.PetCategoryId);
        return View(pet);
    }

    [Route("Edit")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PetModel pet)
    {
        var existed_pet = _context.Pets.Find(pet.Id); //tìm pet theo id pet
        if (existed_pet == null)
        {
            return NotFound();
        }
        ViewBag.Categories = new SelectList(_context.PetCategory, "Id", "Name", pet.PetCategoryId);

        if (ModelState.IsValid)
        {
            pet.Slug = pet.Name.Replace(" ", "-");

            if (pet.ImageUpload != null)
            {
                if (pet.ImageUpload.Length > 5 * 1024 * 1024) // Giới hạn 5MB
                {
                    ModelState.AddModelError("", "File ảnh không được lớn hơn 5MB.");
                    return View(pet);
                }
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/pets");
                if (!Directory.Exists(uploadsDir))
                {
                    Directory.CreateDirectory(uploadsDir);
                }
                string baseName = Path.GetFileNameWithoutExtension(pet.ImageUpload.FileName);
                if (string.IsNullOrEmpty(baseName))
                {
                    baseName = "pet_" + pet.Slug;
                }
                baseName = baseName.Length > 30 ? baseName[..30] : baseName;
                string imageName = baseName + "_" + Guid.NewGuid().ToString() + Path.GetExtension(pet.ImageUpload.FileName);
                string filePath = Path.Combine(uploadsDir, imageName);

                var fs = new FileStream(filePath, FileMode.Create);
                await pet.ImageUpload.CopyToAsync(fs);
                fs.Close();
                var oldImage = Path.Combine(uploadsDir, existed_pet.Image);
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
                existed_pet.Image = imageName;
            }

            existed_pet.Name = pet.Name;
            existed_pet.Description = pet.Description;
            existed_pet.Sex = pet.Sex;
            existed_pet.PetCategoryId = pet.PetCategoryId;
            existed_pet.Age = pet.Age;
            _context.Update(existed_pet);
            await _context.SaveChangesAsync();
            TempData["success"] = "Cập nhật thu cung thành công";
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

    [HttpPost]
    public async Task<IActionResult> Delete(long Id)
    {
        PetModel? pet = await _context.Pets.FindAsync(Id);
        if (pet == null)
        {
            return NotFound();
        }
        string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/pets");
        string oldfilePath = Path.Combine(uploadsDir, pet.Image);
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
        _context.Pets.Remove(pet);
        await _context.SaveChangesAsync();
        TempData["success"] = "Thu cung đã được xóa thành công";
        return RedirectToAction("Index");
    }
}
