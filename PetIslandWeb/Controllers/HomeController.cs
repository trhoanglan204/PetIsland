using PetIsland.DataAccess.Repository.IRepository;
using PetIsland.DataAccess.Data;
using PetIsland.Models;
using PetIsland.Models.ViewModels;
using PetIsland.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

#pragma warning disable IDE0290

namespace PetIslandWeb.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private static List<GroupMemberModel>? ListMembers;
    //private readonly UserManager<AppUserModel> _userManager;

    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        //_userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        //var viewModel = new HomeViewModel
        //{
        //    Products = await _unitOfWork.Product.GetAllAsync(includeProperties: "ProductCategory,ProductImages"),
        //    Pets = await _unitOfWork.Pet.GetAllAsync(includeProperties: "PetCategory,PetImages")
        //};
        //var slider = (await _unitOfWork.Slider.GetAllAsync(s => s.Status == 1)).ToList();
        //ViewBag.Slider = slider;
        //return View(viewModel);
        return View();
    }

    public async Task<IActionResult> Details(int productId)
    {
        var product = await _unitOfWork.Product.GetAsync(
            u => u.Id == productId,
            includeProperties: "Category,ProductImages"
        );
        if (product == null)
        {
            return NotFound();
        }
        ShoppingCart cart = new()
        {
            Product = product,
            Count = 1,
            ProductId = productId

        };
        return View(cart);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Details(ShoppingCart shoppingCart)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        shoppingCart.ApplicationUserId = userId;
        ShoppingCart? cartFromDb = await _unitOfWork.ShoppingCart.GetAsync(u => u.ApplicationUserId == userId && u.ProductId == shoppingCart.ProductId);
        if (cartFromDb != null)
        {
            //shopping cart exists
            cartFromDb.Count += shoppingCart.Count;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
            await _unitOfWork.SaveAsync();
        }
        else
        {
            //add cart record
            _unitOfWork.ShoppingCart.Add(shoppingCart);
            await _unitOfWork.SaveAsync();
            var cartItems = await _unitOfWork.ShoppingCart.GetAllAsync(u => u.ApplicationUserId == userId);
            HttpContext.Session.SetInt32(SD.SessionCart,cartItems.Count());
        }
        TempData["success"] = "Cart updated successfully";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> AddWishlist(int Id, WishlistModel wishlistmodel)
    {
        //var user = await _userManager.GetUserAsync(User);

        //var wishlistProduct = new WishlistModel
        //{
        //    ProductId = Id,
        //    UserId = user!.Id
        //};

        //_unitOfWork.Wishlist.Add(wishlistProduct);
        //try
        //{
        //    await _unitOfWork.SaveAsync();
        //    return Ok(new { success = true, message = "Add to wishlisht Successfully" });
        //}
        //catch (Exception)
        //{
        //    return StatusCode(500, "An error occurred while adding to wishlist table.");
        //}
        return View();

    }

    public async Task<IActionResult> DeleteWishlist(int Id)
    {
        WishlistModel? wishlist = await _unitOfWork.Wishlist.GetAsync(s => s.Id == Id);
        if (wishlist == null)
        {
            return NotFound();
        }

        _unitOfWork.Wishlist.Remove(wishlist);
        await _unitOfWork.SaveAsync();

        TempData["success"] = "Wishlist remove successfull";
        return RedirectToAction("Wishlist", "Home");
    }

    public async Task<IActionResult> Wishlist()
    {
        var wishlist_product = await _unitOfWork.Wishlist.GetWishlistModels();
        return View(wishlist_product);
    }

    public IActionResult Contact()
    {
        return View();
    }

    public IActionResult Search()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Search(string searchString)
    {
        var pets = await _unitOfWork.Pet.GetAllAsync(u => u.Name.Contains(searchString) || u.Description.Contains(searchString));
        var products = await _unitOfWork.Product.GetAllAsync(u => u.Name.Contains(searchString) || u.Description.Contains(searchString));
        if (pets == null && products == null)
        {
            return NotFound();
        }
        var searchResult = new SearchViewModel
        {
            Products = products,
            Pets = pets!,
            SearchKey = searchString
        };
        return View(searchResult);
    }

    public IActionResult MembersInfo()
    {
        ListMembers ??=
            [
                new GroupMemberModel
                {
                    Name = "Nguyen Thi Hong Anh",
                    MSSV = "AT19N0101",
                    ImageUrl = "shin_heart.jpg",
                    Nickname = "anhnottham"
                },
                new GroupMemberModel
                {
                    Name = "Nguyen Anh Khoi",
                    MSSV = "AT19N0121",
                    ImageUrl = "shin_uncencored.jpg",
                },
                new GroupMemberModel
                {
                    Name = "Truong Hoang Lan",
                    MSSV = "AT19N0123",
                    ImageUrl = "shin_dev.jpg",
                    Nickname = "hlaan"
                },
                new GroupMemberModel
                {
                    Name = "Truong Van Thieu",
                    MSSV = "AT19N0138",
                    ImageUrl = "shin_sleep.jpg",
                    Nickname = "TvT"
                }
            ];
        return View(ListMembers);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int statuscode)
    {
        if (statuscode == 404)
        {
            return View("NotFound");
        }
        else
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}