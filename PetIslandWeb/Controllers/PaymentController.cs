using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetIsland.Models;
using PetIsland.Models.Vnpay;
using PetIslandWeb.Services.Momo;
using PetIslandWeb.Services.Paypal;
using PetIslandWeb.Services.Vnpay;

namespace PetIslandWeb.Controllers;

#pragma warning disable IDE0290

public class PaymentController : Controller
{
	private readonly IVnPayService _vnPayService;
	private readonly IPaypalService _paypalService;
    private readonly IMomoService _momoService;

    public PaymentController(IMomoService momoService, IVnPayService vnPayService, IPaypalService paypalService)
	{
		_momoService = momoService;
		_vnPayService = vnPayService;
		_paypalService = paypalService;
    }

	[HttpPost]
	[Authorize]
	public async Task<IActionResult> CreatePaymentMomo(OrderInfo model)
	{
		var response = await _momoService.CreatePaymentAsync(model);
		return Redirect(response!.PayUrl!);
	}

	[HttpPost]
    [Authorize]
    public IActionResult CreatePaymentUrlVnpay(PaymentInformationModel model)
	{
		var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

		return Redirect(url);
	}

	[Authorize]
	[HttpPost]
	public async Task<IActionResult> CreatePaymentPaypal(string Amount)
	{
		try
		{
			var OrderIdRef = "DH" + DateTime.Now.Ticks.ToString();
			var vndAmount = decimal.Parse(Amount);
			var usdAmount = (vndAmount / 25000m).ToString("F2");
			var response = await _paypalService.CreateOrder(usdAmount, "USD", OrderIdRef);
			return Ok(response);
		}
		catch (Exception ex)
		{
			var error = new { ex.GetBaseException().Message };
			return BadRequest(error);
		}
	}
}
