﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetIsland.DataAccess.Data;
using PetIsland.Models;
using PetIsland.Utility;

#pragma warning disable IDE0290

namespace PetIslandWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Route("Admin/Coupon")]
[Authorize(Roles = SD.Role_Admin)]
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
        ViewBag.Total = coupon_list.Count;
        return View();
    }
    [Route("Create")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CouponModel coupon)
    {
        if (ModelState.IsValid)
        {
            coupon.Name = coupon.Name.Replace(" ", "");//coupon không có khoảng trắng
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

    public async Task<IActionResult> Delete(int id)
    {
        var coupon = await _context.Coupons.FindAsync(id);
        if (coupon == null)
        {
            return NotFound();
        }
        _context.Coupons.Remove(coupon);
        await _context.SaveChangesAsync();
        TempData["success"] = "Coupon đã được xóa thành công";
        return RedirectToAction("Index");
    }
}
