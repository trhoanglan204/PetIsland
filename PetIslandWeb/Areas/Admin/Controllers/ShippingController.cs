using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetIsland.DataAccess.Data;
using PetIsland.Models;
using PetIsland.Utility;

#pragma warning disable IDE0290

namespace PetIslandWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Route("Admin/Shipping")]
[Authorize(Roles = $"{SD.Role_Employee},{SD.Role_Admin}")]
public class ShippingController : Controller
{
    private readonly ApplicationDbContext _context;
    public ShippingController(ApplicationDbContext context)
    {
        _context = context;
    }
    [Route("Index")]
    public async Task<IActionResult> Index()
    {
        var shippingList = await _context.Shippings.ToListAsync();
        ViewBag.Shippings = shippingList;
        return View();
    }
    [HttpPost]
    [Route("StoreShipping")]

    public async Task<IActionResult> StoreShipping(ShippingModel shippingModel, string phuong, string quan, string tinh, decimal price)
    {

        shippingModel.City = tinh;
        shippingModel.District = quan;
        shippingModel.Ward = phuong;
        shippingModel.Price = price;

        try
        {
            
            var existingShipping = await _context.Shippings
                .AnyAsync(x => x.City == tinh && x.District == quan && x.Ward == phuong);

            if (existingShipping)
            {
                return Ok(new { duplicate = true, message = "Dữ liệu trùng lặp." });
            }
            _context.Shippings.Add(shippingModel);
            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Thêm shipping thành công" });
        }
        catch (Exception)
        {

            return StatusCode(500, "An error occurred while adding shipping.");
        }
    }
    public async Task<IActionResult> Delete(int Id)
    {
        ShippingModel shipping = await _context.Shippings.FindAsync(Id);

        _context.Shippings.Remove(shipping);
        await _context.SaveChangesAsync();
        TempData["success"] = "Shipping đã được xóa thành công";
        return RedirectToAction("Index");
    }

}
