using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetIsland.DataAccess.Data;
using PetIsland.Models;

#pragma warning disable IDE0290

namespace PetIslandWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Route("Admin/Coupon")]
[Authorize(Roles = "Publisher,Author,Admin")]
public class CouponController : Controller
{
    private readonly ApplicationDbContext _context;
    public CouponController(ApplicationDbContext context)
    {
        _context = context;
    }
    [Route("Index")]
    public async Task<IActionResult> Index()
    {
        var coupon_list = await _context.Coupons.ToListAsync();
        ViewBag.Coupons = coupon_list;
        return View();
    }
    [Route("Create")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CouponModel coupon)
    {


        if (ModelState.IsValid)
        {

            _context.Add(coupon);
            await _context.SaveChangesAsync();
            TempData["success"] = "Thêm coupon thành công";
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
}
