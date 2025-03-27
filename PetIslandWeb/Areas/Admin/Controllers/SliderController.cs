using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetIsland.DataAccess.Data;
using PetIsland.Models;

#pragma warning disable IDE0290

namespace PetIslandWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Route("Admin/Slider")]
[Authorize(Roles = "Publisher,Author,Admin")]
public class SliderController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostEnviroment;
    public SliderController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnviroment = webHostEnvironment;
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
                string uploadsDir = Path.Combine(_webHostEnviroment.WebRootPath, "media/sliders");
                string imageName = Guid.NewGuid().ToString() + "_" + slider.ImageUpload.FileName;
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
        SliderModel slider = await _context.Sliders.FindAsync(Id);

        return View(slider);
    }
    [Route("Edit")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(SliderModel slider)
    {
        var slider_existed = _context.Sliders.Find(slider.Id);
        if (ModelState.IsValid)
        {

            if (slider.ImageUpload != null)
            {
                string uploadsDir = Path.Combine(_webHostEnviroment.WebRootPath, "media/sliders");
                string imageName = Guid.NewGuid().ToString() + "_" + slider.ImageUpload.FileName;
                string filePath = Path.Combine(uploadsDir, imageName);

                FileStream fs = new FileStream(filePath, FileMode.Create);
                await slider.ImageUpload.CopyToAsync(fs);
                fs.Close();
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
        SliderModel slider = await _context.Sliders.FindAsync(Id);
        if (!string.Equals(slider.Image, "noname.jpg"))
        {
            string uploadsDir = Path.Combine(_webHostEnviroment.WebRootPath, "media/sliders");
            string oldfilePath = Path.Combine(uploadsDir, slider.Image);
            if (System.IO.File.Exists(oldfilePath))
            {
                System.IO.File.Delete(oldfilePath);
            }
        }

        _context.Sliders.Remove(slider);
        await _context.SaveChangesAsync();
        TempData["success"] = "Slider đã được xóa thành công";
        return RedirectToAction("Index");
    }
}
