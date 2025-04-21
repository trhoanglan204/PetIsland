using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetIsland.DataAccess.Data;
using PetIsland.Models;
using PetIsland.Models.ORS;
using PetIslandWeb.Services.ORS;

namespace PetIslandWeb.Areas.Admin.Controllers;

#pragma warning disable IDE0290

[Area("Admin")]
[Route("Admin/Contact")]
[Authorize(Roles = "Admin")]
public class ContactController : Controller
{
    private readonly ApplicationDbContext _dataContext;
    private readonly GeocodingService _geoService;
    public ContactController(ApplicationDbContext context, GeocodingService geoService)
    {
        _dataContext = context;
        _geoService = geoService;
    }

    [Route("Index")]
    public async Task<IActionResult> Index()
    {
        var contact = await _dataContext.Contact.FirstOrDefaultAsync();
        if (contact != null)
        {
            var key = contact.ORS_Key;
            if (key != null && (contact.ORS_lon==0 || contact.ORS_lat==0))
            {
                var ORS = await _geoService.GeocodeSearchAsync(contact.Address!);
                if (ORS != null)
                {
                    contact.ORS_lon = ORS.lon;
                    contact.ORS_lat = ORS.lat;
                    _dataContext.Update(contact);
                    await _dataContext.SaveChangesAsync();
                }
                else
                {
                    TempData["error"] = "Cần cập nhật lại key";
                }
            }
        }
        return View(contact);
    }

    [Route("Edit")]
    public async Task<IActionResult> Edit()
    {
        var contact = await _dataContext.Contact.FirstOrDefaultAsync();
        return View(contact ?? new ContactModel());
    }

    [Route("Edit")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ContactModel contact)
    {
        if (ModelState.IsValid)
        {
            var existed_contact = _dataContext.Contact.FirstOrDefault();
            if (existed_contact == null)
            {
                TempData["error"] = "Tạo thông tin liên hệ thành công.";
                _dataContext.Contact.Add(contact);
            }
            else
            {
                existed_contact.Name = contact.Name;
                existed_contact.Email = contact.Email;
                existed_contact.Phone = contact.Phone;
                existed_contact.Address = contact.Address;
                var ORS = await _geoService.GeocodeSearchAsync(contact.Address!);
                if (ORS != null)
                {
                    existed_contact.ORS_lon = ORS.lon;
                    existed_contact.ORS_lat = ORS.lat;
                }
                else
                {
                    TempData["error"] = "Cần cập nhật lại key";
                }
                _dataContext.Update(existed_contact);
                TempData["success"] = "Cập nhật thông tin liên hệ thành công";
            }
            await _dataContext.SaveChangesAsync();
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

    [HttpGet]
    [Route("UpdateORSKey")]
    public async Task<IActionResult> UpdateORSKey()
    {
        var contact = await _dataContext.Contact.FirstOrDefaultAsync();
        if (contact == null)
        {
            TempData["error"] = "Không tìm thấy thông tin để cập nhật ORS Key.";
            return RedirectToAction("Index");
        }
        return View(contact);
    }

    [HttpPost]
    [Route("UpdateORSKey")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateORSKey(ContactModel model)
    {
        var contact = await _dataContext.Contact.FirstOrDefaultAsync();
        if (contact == null)
        {
            TempData["error"] = "Không tìm thấy thông tin để cập nhật ORS Key.";
            return RedirectToAction("Index");
        }

        contact.ORS_Key = model.ORS_Key!;
        _geoService.SetKey(model.ORS_Key!);

        var ORS = await _geoService.GeocodeSearchAsync(model.Address!);
        if (ORS != null)
        {
            contact.ORS_lon = ORS.lon;
            contact.ORS_lat = ORS.lat;
        }
        else
        {
            TempData["error"] = "Cần cập nhật lại key";
        }

        _dataContext.Contact.Update(contact);
        await _dataContext.SaveChangesAsync();

        TempData["success"] = "Cập nhật ORS Key thành công";
        return RedirectToAction("Index");
    }
}
