using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PetIsland.DataAccess.Data;
using PetIsland.Models;
using PetIsland.Utility;
using PetIslandWeb.Services.Vnpay;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.UI.Services;
using PetIsland.Models.Momo;
using PetIsland.Models.Vnpay;
using PetIslandWeb.Services.Momo;

#pragma warning disable IDE0290

namespace PetIslandWeb.Controllers;

public class CheckoutController : Controller
{

	private readonly ApplicationDbContext _dataContext;
	private readonly IEmailSender _emailSender;
	private readonly IVnPayService _vnPayService;
    private readonly IMomoService _momoService;
    public CheckoutController(IEmailSender emailSender, ApplicationDbContext context, IVnPayService vnPayService, IMomoService momoService)
	{
		_dataContext = context;
		_emailSender = emailSender;
		_vnPayService = vnPayService;
        _momoService = momoService;
    }
    public IActionResult Index()
	{
		return View();
	}
	public async Task<IActionResult> Checkout(string? paymentMethod, string? paymentId)
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

            //Nhận coupon code from cookie
            var coupon_code = Request.Cookies["CouponTitle"];
            var couponDiscountPriceCookie = Request.Cookies["CouponDiscountPrice"];
            decimal couponDiscountPrice = 0;

            if (!string.IsNullOrEmpty(couponDiscountPriceCookie))
            {
                couponDiscountPrice = decimal.Parse(couponDiscountPriceCookie);
            }
            if (shippingPriceCookie != null)
			{
				var shippingPriceJson = shippingPriceCookie;
				shippingPrice = JsonConvert.DeserializeObject<decimal>(shippingPriceJson);
			}

            List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? [];
            decimal cartTotal = cartItems.Sum(x => x.Quantity * x.Price);

            orderItem.GrandTotal = cartTotal + shippingPrice - couponDiscountPrice;
            orderItem.ShippingCost = shippingPrice;
			orderItem.CouponCode = coupon_code;
            orderItem.PaymentMethod = paymentMethod + " " + paymentId;

            _dataContext.Add(orderItem);
			_dataContext.SaveChanges();

			//tạo order detail
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
    public async Task<IActionResult> PaymentCallBack()
    {
        var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
        var requestQuery = HttpContext.Request.Query;
        if (requestQuery["resultCode"] != 0) //test -> giao dich k thanh cong luu db
        {
            var newMomoInsert = new MomoInfoModel
            {
                OrderId = requestQuery["orderId"],
                FullName = User.FindFirstValue(ClaimTypes.Email),
                Amount = decimal.Parse(requestQuery["Amount"]),
                OrderInfo = requestQuery["orderInfo"],
                DatePaid = DateTime.Now,
            };
            _dataContext.MomoInfo.Add(newMomoInsert);
            await _dataContext.SaveChangesAsync();
            await Checkout("MOMO",requestQuery["orderId"]);
        }
        else
        {
            TempData["success"] = "Giao dịch Momo không thành công";
            return RedirectToAction("Index", "Cart");
        }
        return View(response);
    }

    [HttpGet]
	public async Task<IActionResult> PaymentCallbackVnpay()
	{
		var response = _vnPayService.PaymentExecute(Request.Query);
        if (response.VnPayResponseCode == "00") //test -> giao dich thanh cong luu db
        {
            var newVnpayInsert = new VnpayInfoModel
            {
                OrderId = response.OrderId,
                OrderDescription = response.OrderDescription,
                TransactionId = response.TransactionId,
                PaymentMethod = response.PaymentMethod,
                PaymentId = response.PaymentId,
                DatePaid = DateTime.Now,
            };

            _dataContext.VnpayInfo.Add(newVnpayInsert);
            await _dataContext.SaveChangesAsync();
            await Checkout(response.PaymentMethod, response.PaymentId);
        }
        else
        {
            TempData["success"] = "Giao dịch Vnpay không thành công";
            return RedirectToAction("Index", "Cart");
        }
        return Json(response);
	}
}
