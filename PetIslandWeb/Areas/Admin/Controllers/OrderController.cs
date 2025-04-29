using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using PetIsland.DataAccess.Data;
using PetIsland.Utility;
using PetIsland.Models;

#pragma warning disable IDE0290

namespace PetIslandWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Route("Admin/Order")]
[Authorize(Roles = $"{SD.Role_Admin},{SD.Role_Employee}")]
public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;
    public OrderController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("Index")]
    public async Task<IActionResult> Index(int pg = 1)
    {
        var orderList = await _context.Orders.OrderByDescending(p => p.Id).ToListAsync();
        const int pageSize = 10;
        if (pg < 1)
        {
            pg = 1;
        }

        int resCount = orderList.Count;
        var pager = new Paginate(resCount, pg, pageSize);
        int recSkip = (pg - 1) * pageSize;

        var data = orderList.Skip(recSkip).Take(pager.PageSize).ToList();
        ViewBag.Pager = pager;
        return View(data);
    }

    [HttpGet]
    [Route("ViewOrder")]
    public async Task<IActionResult> ViewOrder(string ordercode)
    {
        var DetailsOrder = await _context.OrderDetails.Include(od => od.Product)
            .Where(od => od.OrderCode == ordercode).ToListAsync();

        var Order = _context.Orders.Where(o => o.OrderCode == ordercode).First();

        ViewBag.ShippingCost = Order.ShippingCost;
        ViewBag.Status = Order.Status;
        return View(DetailsOrder);
    }

    [HttpGet]
    [Route("PaymentMomoInfo")]
    public async Task<IActionResult> PaymentMomoInfo(string orderId)
    {
        var momoInfo = await _context.MomoInfo.FirstOrDefaultAsync(m => m.OrderId == orderId);
        if (momoInfo == null)
        {
            return NotFound();
        }
        return View(momoInfo);
    }

    [HttpGet]
    [Route("PaymentVnpayInfo")]
    public async Task<IActionResult> PaymentVnpayInfo(string orderId)
    {
        var vnpayInfo = await _context.VnpayInfo.FirstOrDefaultAsync(m => m.PaymentId == orderId);
        if (vnpayInfo == null)
        {
            return NotFound();
        }
        return View(vnpayInfo);
    }

    [HttpPost]
    [Route("UpdateOrder")]
    public async Task<IActionResult> UpdateOrder(string ordercode, int status)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderCode == ordercode);

        if (order == null)
        {
            return NotFound();
        }

        order.Status = status;

        try
        {
            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Order status updated successfully" });
        }
        catch (Exception)
        {


            return StatusCode(500, "An error occurred while updating the order status.");
        }
    }
    [HttpGet]
    [Route("Delete")]
    public async Task<IActionResult> Delete(string ordercode)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderCode == ordercode);

        if (order == null)
        {
            return NotFound();
        }
        try
        {

            //delete order
            _context.Orders.Remove(order);


            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        catch (Exception)
        {

            return StatusCode(500, "An error occurred while deleting the order.");
        }
    }
}
