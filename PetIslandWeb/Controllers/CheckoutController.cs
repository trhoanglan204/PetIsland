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
using PetIsland.Models.Paypal;
using PetIslandWeb.Services.Momo;
using PetIslandWeb.Services.Paypal;

#pragma warning disable IDE0290

namespace PetIslandWeb.Controllers;

public class CheckoutController : Controller
{

	private readonly ApplicationDbContext _dataContext;
	private readonly IEmailSender _emailSender;
	private readonly IVnPayService _vnPayService;
    private readonly IMomoService _momoService;
    private readonly IPaypalService _paypalService;
    public CheckoutController(IEmailSender emailSender, ApplicationDbContext context, IVnPayService vnPayService, IMomoService momoService, IPaypalService paypalService)
	{
		_dataContext = context;
		_emailSender = emailSender;
		_vnPayService = vnPayService;
        _momoService = momoService;
        _paypalService = paypalService;
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

			TempData["success"] = "Đơn hàng đã được tạo";
			return RedirectToAction("History", "Account");
		}
	}

    [HttpGet]
    public async Task<IActionResult> PaymentCallBackMomo()
    {
        var requestQuery = HttpContext.Request.Query;
        var response = _momoService.PaymentExecute(requestQuery);
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

    [HttpPost]
    public async Task<IActionResult> CapturePaypalOrder(string orderID, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _paypalService.CaptureOrder(orderID);

            if (response == null || response.status != "COMPLETED")
            {
                TempData["success"] = "Giao dịch Paypal không thành công";
                return RedirectToAction("Index", "Cart");
            }

            var usdAmount = decimal.Parse(response.purchase_units.First().payments.captures.First().amount.value);
            var vndAmout = (usdAmount * 25000m);

            // Lưu database đơn hàng của mình
            var newPaypalInsert = new PaypalInfoModel
            {
                OrderId = response.id,
                status = response.status,
                FullName = User.FindFirstValue(ClaimTypes.Email),
                Amount = vndAmout,
                OrderInfo = "Thanh toán qua Paypal",
                DatePaid = DateTime.Now,
            };

            _dataContext.PaypalInfo.Add(newPaypalInsert);
            await _dataContext.SaveChangesAsync(cancellationToken);
            await Checkout("Paypal", response.id);

            return Json(new { orderId = newPaypalInsert.OrderId });
        }
        catch (Exception ex)
        {
            var error = new { ex.GetBaseException().Message };
            return BadRequest(error);
        }
    }


    [HttpGet]
    public  async Task<IActionResult> PaymentCallbackPaypal(string orderId)
    {
        var order = await _dataContext.PaypalInfo.FirstOrDefaultAsync(o => o.OrderId == orderId);
        if(order == null)
        {
            return View("Error");
        }
        if (order.status == "COMPLETED")
        {
            return View(order);
        }
        else
        {
            TempData["success"] = "Giao dịch Paypal không thành công";
            return RedirectToAction("Index", "Cart");
        }
    }
}
