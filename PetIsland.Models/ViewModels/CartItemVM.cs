﻿namespace PetIsland.Models.ViewModels;

public class CartItemVM
{
	public List<CartItemModel> CartItems { get; set; }
	public decimal GrandTotal { get; set; }

	public decimal ShippingPrice { get; set; }

	public string CouponCode { get; set; }
}
