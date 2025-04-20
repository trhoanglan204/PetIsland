using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PetIsland.DataAccess.Data;
using PetIsland.Models;
using PetIsland.Utility;
using PetIslandWeb.Services.Vnpay;
using System.Security.Claims;

#pragma warning disable IDE0290

namespace PetIslandWeb.Controllers;

public class CheckoutController : Controller
{

	private readonly ApplicationDbContext _dataContext;
	private readonly EmailSender _emailSender;
	private readonly IVnPayService _vnPayService;
	public CheckoutController(EmailSender emailSender, ApplicationDbContext context, IVnPayService vnPayService)
	{
		_dataContext = context;
		_emailSender = emailSender;
		_vnPayService = vnPayService;

	}
	public IActionResult Index()
	{
		return View();
	}
	public async Task<IActionResult> Checkout()
	{
		var userEmail = User.FindFirstValue(ClaimTypes.Email);
		if (userEmail == null)
		{
			return RedirectToAction("Login", "Account");
		}
		else
		{
			var ordercode = Guid.NewGuid().ToString();
            var orderItem = new OrderModel
            {
                OrderCode = ordercode,
                UserName = userEmail,
                Status = 1,
                CreatedDate = DateTime.Now
            };
            // Retrieve shipping price from cookie
            var shippingPriceCookie = Request.Cookies["ShippingPrice"];
			decimal shippingPrice = 0;

			if (shippingPriceCookie != null)
			{
				var shippingPriceJson = shippingPriceCookie;
				shippingPrice = JsonConvert.DeserializeObject<decimal>(shippingPriceJson);
			}
			orderItem.ShippingCost = shippingPrice;
			//Nhận coupon code
			var CouponCode = Request.Cookies["CouponTitle"];
			orderItem.CouponCode = CouponCode;
			_dataContext.Add(orderItem);
			_dataContext.SaveChanges();
			//tạo order detail
			List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? [];
			foreach (var cart in cartItems)
			{
                var orderdetail = new OrderDetail
                {
                    UserName = userEmail,
                    OrderCode = ordercode,
                    ProductId = cart.ProductId,
                    Price = cart.Price,
                    Quantity = cart.Quantity
                };
                //update product quantity
                var product = await _dataContext.Products.Where(p => p.Id == cart.ProductId).FirstAsync();
				product.Quantity -= cart.Quantity;
				product.SoldOut += cart.Quantity;
				_dataContext.Update(product);
				_dataContext.Add(orderdetail);
				_dataContext.SaveChanges();
			}
			HttpContext.Session.Remove("Cart");
            //Send mail order when success
            if (ClaimTypes.Role != SD.Role_Admin && ClaimTypes.Role != SD.Role_Employee)
            {
                var receiver = userEmail;
                if (!string.IsNullOrEmpty(receiver))
                {
                    var subject = "Đặt hàng thành công";
                    var message = "Đặt hàng thành công, trải nghiệm dịch vụ nhé.";
                    await _emailSender.SendEmailAsync(receiver, subject, message);
                }
            }

			TempData["success"] = "Đơn hàng đã được tạo,vui lòng chờ duyệt đơn hàng nhé.";
			return RedirectToAction("History", "Account");
		}
	}

	[HttpGet]
	public IActionResult PaymentCallbackVnpay()
	{
		var response = _vnPayService.PaymentExecute(Request.Query);

		return Json(response);
	}
}
