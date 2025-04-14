using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using PetIsland.DataAccess.Data;
using PetIsland.Utility;

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
    public async Task<IActionResult> Index()
    {
        return View(await _context.Orders.OrderByDescending(p => p.Id).ToListAsync());
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
