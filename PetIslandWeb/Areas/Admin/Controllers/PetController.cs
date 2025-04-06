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
//[Authorize(Roles = SD.Role_Admin)]
//[Authorize(Roles = SD.Role_Employee)]
[Route("Admin/Pet")]
public class PetController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public PetController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    [Route("Index")]
    public async Task<IActionResult> Index()
    {
        var objCatagoryList = await _context.Pets.OrderByDescending(p => p.Id).Include(c => c.PetCategory).ToListAsync();
        return View(objCatagoryList);
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
                ModelState.AddModelError("", "Thu cung đã có trong database");
                return View(pet);
            }

            if (pet.ImageUpload != null)
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/pets");
                string imageName = Guid.NewGuid().ToString() + "_" + pet.ImageUpload.FileName;
                string filePath = Path.Combine(uploadsDir, imageName);

                var fs = new FileStream(filePath, FileMode.Create);
                await pet.ImageUpload.CopyToAsync(fs);
                fs.Close();
                pet.Image = imageName;
            }

            _context.Add(pet);
            await _context.SaveChangesAsync();
            TempData["success"] = "Thêm thu cung thành công";
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
        var existed_pet = _context.Pets.Find(pet.Id)!; //tìm pet theo id pet
        ViewBag.Categories = new SelectList(_context.PetCategory, "Id", "Name", pet.PetCategoryId);

        if (ModelState.IsValid)
        {
            pet.Slug = pet.Name.Replace(" ", "-");

            if (pet.ImageUpload != null)
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/pets");
                string imageName = Guid.NewGuid().ToString() + "_" + pet.ImageUpload.FileName;
                string filePath = Path.Combine(uploadsDir, imageName);

                var fs = new FileStream(filePath, FileMode.Create);
                await pet.ImageUpload.CopyToAsync(fs);
                fs.Close();
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
        if (!string.Equals(pet.Image, "null.jpg"))
        {
            string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/pets");
            string oldfilePath = Path.Combine(uploadsDir, pet.Image);
            if (System.IO.File.Exists(oldfilePath))
            {
                System.IO.File.Delete(oldfilePath);
            }
        }
        _context.Pets.Remove(pet);
        await _context.SaveChangesAsync();
        TempData["success"] = "Thu cung đã được xóa thành công";
        return RedirectToAction("Index");
    }
}
