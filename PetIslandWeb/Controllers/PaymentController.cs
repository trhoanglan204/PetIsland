﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PetIsland.Models;
using PetIsland.Models.Vnpay;
using PetIslandWeb.Services.Momo;
using PetIslandWeb.Services.Vnpay;
using System.Security.Cryptography;

namespace PetIslandWeb.Controllers;


public class PaymentController : Controller
{
	private readonly IVnPayService _vnPayService;
	private IMomoService _momoService;
	public PaymentController(IMomoService momoService, IVnPayService vnPayService)
	{
		_momoService = momoService;
		_vnPayService = vnPayService;
	}
	[HttpPost]
	public async Task<IActionResult> CreatePaymentMomo(OrderInfo model)
	{
		var response = await _momoService.CreatePaymentAsync(model);
		return Redirect(response.PayUrl);
	}


	[HttpGet]
	public IActionResult PaymentCallBack()
	{
		var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
		return View(response);
	}
	[HttpPost]
	public IActionResult CreatePaymentUrlVnpay(PaymentInformationModel model)
	{
		var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

		return Redirect(url);
	}



}
