using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetIsland.DataAccess.Data;
using PetIsland.Models;
using PetIsland.Utility;

#pragma warning disable IDE0290

namespace PetIslandWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Route("Admin/Slider")]
[Authorize(Roles = $"{SD.Role_Admin},{SD.Role_Employee}")]
public class SliderController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<SliderController> _logger;

    public SliderController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment,ILogger<SliderController> logger)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
    }
    [Route("Index")]
    public async Task<IActionResult> Index()
    {
        return View(await _context.Sliders.OrderByDescending(p => p.Id).ToListAsync());
    }
    [Route("Create")]
    public IActionResult Create()
    {

        return View();
    }
    [Route("Create")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SliderModel slider)
    {

        if (ModelState.IsValid)
        {

            if (slider.ImageUpload != null)
            {
                if (slider.ImageUpload.Length > 5 * 1024 * 1024) // Giới hạn 5MB
                {
                    ModelState.AddModelError("", "File ảnh không được lớn hơn 5MB.");
                    return View(slider);
                }
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/sliders");
                if (!Directory.Exists(uploadsDir))
                {
                    Directory.CreateDirectory(uploadsDir);
                }
                string baseName = Path.GetFileNameWithoutExtension(slider.ImageUpload.FileName);
                if (string.IsNullOrEmpty(baseName))
                {
                    baseName = "default";
                }
                baseName = baseName.Length > 30 ? baseName[..30] : baseName;
                string imageName = baseName + "_" + Guid.NewGuid().ToString() + Path.GetExtension(slider.ImageUpload.FileName);
                string filePath = Path.Combine(uploadsDir, imageName);

                var fs = new FileStream(filePath, FileMode.Create);
                await slider.ImageUpload.CopyToAsync(fs);
                fs.Close();
                slider.Image = imageName;
            }

            _context.Add(slider);
            await _context.SaveChangesAsync();
            TempData["success"] = "Thêm slider thành công";
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
    public async Task<IActionResult> Edit(int Id)
    {
        SliderModel? slider = await _context.Sliders.FindAsync(Id);
        if (slider == null)
        {
            return NotFound();
        }
        return View(slider);
    }

    [Route("Edit")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(SliderModel slider)
    {
        var slider_existed = await _context.Sliders.FindAsync(slider.Id);
        if(slider_existed == null)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            if (slider.ImageUpload != null)
            {
                if (slider.ImageUpload.Length > 5 * 1024 * 1024) // Giới hạn 5MB
                {
                    ModelState.AddModelError("", "File ảnh không được lớn hơn 5MB.");
                    return View(slider);
                }
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/sliders");
                if (!Directory.Exists(uploadsDir))
                {
                    Directory.CreateDirectory(uploadsDir);
                }
                string baseName = Path.GetFileNameWithoutExtension(slider.ImageUpload.FileName);
                if (string.IsNullOrEmpty(baseName))
                {
                    baseName = "default";
                }
                baseName = baseName.Length > 30 ? baseName[..30] : baseName;
                string imageName = baseName + "_" + Guid.NewGuid().ToString() + Path.GetExtension(slider.ImageUpload.FileName);
                string filePath = Path.Combine(uploadsDir, imageName);

                var fs = new FileStream(filePath, FileMode.Create);
                await slider.ImageUpload.CopyToAsync(fs);
                fs.Close();
                var oldImage = Path.Combine(uploadsDir, slider.Image);
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
                slider_existed.Image = imageName;
            }
            slider_existed.Name = slider.Name;
            slider_existed.Description = slider.Description;
            slider_existed.Status = slider.Status;

            _context.Update(slider_existed);
            await _context.SaveChangesAsync();
            TempData["success"] = "Cập nhật slider thành công";
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

    [Route("Delete")]
    public async Task<IActionResult> Delete(int Id)
    {
        SliderModel? slider = await _context.Sliders.FindAsync(Id);
        if (slider == null)
        {
            return NotFound();
        }
        string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/sliders");
        string oldfilePath = Path.Combine(uploadsDir, slider.Image!);
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
        _context.Sliders.Remove(slider);
        await _context.SaveChangesAsync();
        TempData["success"] = "Slider đã được xóa thành công";
        return RedirectToAction("Index");
    }
}
