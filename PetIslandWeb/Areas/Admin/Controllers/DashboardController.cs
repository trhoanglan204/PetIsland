using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;
using PetIsland.Models;
using PetIsland.DataAccess.Data;
using PetIsland.Utility;

namespace PetIslandWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Route("Admin/Dashboard")]
[Authorize(Roles = SD.Role_Admin)]
public class DashboardController : Controller
{
    private const int v = 2024;
    private readonly ApplicationDbContext _dataContext;
    public DashboardController(ApplicationDbContext context)
    {
        _dataContext = context;
    }
    public IActionResult Index()
    {
        var count_product = _dataContext.Products.Count();
        var count_order = _dataContext.Orders.Count();
        var count_productcategory = _dataContext.ProductCategory.Count();
        var count_user = _dataContext.Users.Count();
        ViewBag.CountProduct = count_product;
        ViewBag.CountOrder = count_order;
        ViewBag.CountProductCategory = count_productcategory;
        ViewBag.CountUser = count_user;
        return View();
    }
    [HttpPost]
    [Route("SubmitFilterDate")]
    public IActionResult SubmitFilterDate(string filterdate)
    {
        var dateselect = DateTime.Parse(filterdate).ToString("yyyy-MM-dd");
        var chartData = _dataContext.Orders
       .Where(o => o.CreatedDate.ToString("yyyy-MM-dd") == dateselect) // Optional: Filter by date
      .Join(_dataContext.OrderDetails,
          o => o.OrderCode,
          od => od.OrderCode,
          (o, od) => new StatisticalModel
          {
              date = o.CreatedDate,
              revenue = od.Quantity * od.Price, // Calculate revenue based on order details
              orders = 1 // Assuming each order detail represents one order
          })
      .GroupBy(s => s.date)
      .Select(group => new StatisticalModel
      {
          date = group.Key,
          revenue = group.Sum(s => s.revenue),
          orders = group.Count()
      })
      .ToList();

        return Json(chartData);
    }
    [HttpPost]
    [Route("SelectFilterDate")]
    public IActionResult SelectFilterDate(string filterdate)
    {
        var chartData = new List<StatisticalModel>();
        // Initialize as empty list
        var today = DateTime.Today;
        var month = new DateTime(today.Year, today.Month, 1);
        var first = month.AddMonths(-1);
        var last = month.AddDays(-1);


        if (filterdate == "last_month")
        {
            chartData = _dataContext.Orders
           .Where(o => o.CreatedDate > first && o.CreatedDate < today)

           .Join(_dataContext.OrderDetails,
             o => o.OrderCode,
             od => od.OrderCode,
             (o, od) => new StatisticalModel
             {
                 date = o.CreatedDate,
                 revenue = od.Quantity * od.Price, // Calculate revenue based on order details
                 orders = 1 // Assuming each order detail represents one order
             })
             .GroupBy(s => s.date)
             .Select(group => new StatisticalModel
             {
                 date = group.Key,
                 revenue = group.Sum(s => s.revenue),
                 orders = group.Count()
             })
             .ToList();
        }


        return Json(chartData);
    }
    [HttpPost]
    [Route("GetChartData")]
    public IActionResult GetChartData()
    {

        var chartData = _dataContext.Orders
      .Join(_dataContext.OrderDetails,
          o => o.OrderCode,
          od => od.OrderCode,
          (o, od) => new StatisticalModel
          {
              date = o.CreatedDate,
              revenue = od.Quantity * od.Price, // Calculate revenue based on order details
              orders = 1 // Assuming each order detail represents one order
          })
      .GroupBy(s => s.date)
      .Select(group => new StatisticalModel
      {
          date = group.Key,
          revenue = group.Sum(s => s.revenue),
          orders = group.Count()
      })
      .ToList();

        return Json(chartData);
    }

}
