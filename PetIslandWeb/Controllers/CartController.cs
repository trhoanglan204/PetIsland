using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PetIsland.DataAccess.Data;
using PetIsland.Models;
using PetIsland.Models.ViewModels;
using PetIsland.Utility;

#pragma warning disable IDE0290

namespace PetIslandWeb.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ApplicationDbContext _context;
    public CartController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? [];
        // Nhận shipping giá từ cookie
        var shippingPriceCookie = Request.Cookies["ShippingPrice"];
        decimal shippingPrice = 0;

        if (shippingPriceCookie != null)
        {
            var shippingPriceJson = shippingPriceCookie;
            shippingPrice = JsonConvert.DeserializeObject<decimal>(shippingPriceJson);
        }

        //Nhận Coupon code từ cookie
        var coupon_code = Request.Cookies["CouponTitle"];

        CartItemVM cartVM = new()
        {
            CartItems = cartItems,
            GrandTotal = cartItems.Sum(x => x.Quantity * x.Price),
            ShippingPrice = shippingPrice,
            CouponCode = coupon_code
        };

        return View(cartVM);
    }

    public async Task<IActionResult> Add(long Id, int? quantity)
    {
        ProductModel? product = await _context.Products.FindAsync(Id);
        List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? [];
        CartItemModel? cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();
        if(product == null)
        {
            return NotFound();
        }
        int qtyToAdd = quantity ?? 1;
        if (cartItems == null)
        {
            cart.Add(new CartItemModel(product, qtyToAdd));
        }
        else
        {
            cartItems.Quantity += qtyToAdd;
        }

        HttpContext.Session.SetJson("Cart", cart);

        TempData["success"] = "Add Product to cart Sucessfully!";
        return Redirect(Request.Headers.Referer.ToString());
    }

    public IActionResult Decrease(int Id)
    {
        List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
        CartItemModel? cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();
        if (cartItem == null)
        {
            return NotFound();
        }
        if (cartItem.Quantity > 1)
        {
            --cartItem.Quantity;
        }
        else
        {
            cart.RemoveAll(p => p.ProductId == Id);
        }
        if (cart.Count == 0)
        {
            HttpContext.Session.Remove("Cart");
        }
        else
        {
            HttpContext.Session.SetJson("Cart", cart);
        }

        TempData["success"] = "Decrease Product to cart Sucessfully! ";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Increase(long Id)
    {
        ProductModel? product = await _context.Products.Where(p => p.Id == Id).FirstOrDefaultAsync();

        List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
        CartItemModel? cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();
        if (cartItem == null)
        {
            return NotFound();
        }
        if (product == null)
        {
            return NotFound();
        }
        if (cartItem.Quantity >= 1 && product.Quantity > cartItem.Quantity)
        {
            ++cartItem.Quantity;
            TempData["success"] = "Increase Product to cart Sucessfully! ";
        }
        else
        {
            cartItem.Quantity = product.Quantity;
            TempData["success"] = "Maximum Product Quantity to cart Sucessfully! ";
        }
        if (cart.Count == 0)
        {
            HttpContext.Session.Remove("Cart");
        }
        else
        {
            HttpContext.Session.SetJson("Cart", cart);
        }

        return RedirectToAction("Index");
    }
    public IActionResult Remove(int Id)
    {
        List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
        cart.RemoveAll(p => p.ProductId == Id);
        if (cart.Count == 0)
        {
            HttpContext.Session.Remove("Cart");
        }
        else
        {
            HttpContext.Session.SetJson("Cart", cart);
        }

        TempData["success"] = "Remove Product to cart Sucessfully! ";
        return RedirectToAction("Index");
    }
    public IActionResult Clear()
    {
        HttpContext.Session.Remove("Cart");

        TempData["success"] = "Clear all Product to cart Sucessfully! ";
        return RedirectToAction("Index");
    }
    [HttpPost]
    [Route("Cart/GetShipping")]
    public async Task<IActionResult> GetShipping(string tinh, string quan, string phuong)
    {

        var existingShipping = await _context.Shippings
            .FirstOrDefaultAsync(x => x.City == tinh && x.District == quan && x.Ward == phuong);

        decimal shippingPrice = 0; // Set mặc định giá tiền

        if (existingShipping != null)
        {
            shippingPrice = existingShipping.Price;
        }
        else
        {
            //Set mặc định giá tiền nếu ko tìm thấy
            shippingPrice = 50000;
        }
        var shippingPriceJson = JsonConvert.SerializeObject(shippingPrice);
        try
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                Secure = true // using HTTPS
            };

            Response.Cookies.Append("ShippingPrice", shippingPriceJson, cookieOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding shipping price cookie: {ex.Message}");
        }
        return Json(new { shippingPrice });
    }

    [HttpPost]
    [Route("Cart/GetCoupon")]
    public async Task<IActionResult> GetCoupon(string coupon_value)
    {
        var validCoupon = await _context.Coupons
            .FirstOrDefaultAsync(x => x.Name == coupon_value && x.Quantity >= 1);
        if (validCoupon == null)
        {
            //mean no coupon available
            return Ok(new { success = false, message = "Coupon not existed" });
        }
        string couponTitle = validCoupon.Name + " | " + validCoupon.Description;
        TimeSpan remainingTime = validCoupon.DateExpired - DateTime.Now;
        int daysRemaining = remainingTime.Days;

        if (daysRemaining >= 0)
        {
            try
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                    Secure = true,
                    SameSite = SameSiteMode.Strict // Kiểm tra tính tương thích trình duyệt
                };

                Response.Cookies.Append("CouponTitle", couponTitle, cookieOptions);
                return Ok(new { success = true, message = "Coupon applied successfully" });
            }
            catch (Exception ex)
            {
                //trả về lỗi 
                Console.WriteLine($"Error adding apply coupon cookie: {ex.Message}");
                return Ok(new { success = false, message = "Coupon applied failed" });
            }
        }
        else
        {
            return Ok(new { success = false, message = "Coupon has expired" });
        }
    }

    [HttpPost]
    [Route("Cart/RemoveShippingCookie")]
    public IActionResult RemoveShippingCookie()
    {
        Response.Cookies.Delete("ShippingPrice");
        return Json(new { success = true });
    }
}
