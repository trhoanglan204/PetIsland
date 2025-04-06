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
using Microsoft.EntityFrameworkCore;

#pragma warning disable IDE0290

namespace PetIslandWeb.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    //private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;
    private static List<GroupMemberModel>? ListMembers;
    private readonly UserManager<AppUserModel> _userManager;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<AppUserModel> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new HomeViewModel
        {
            Products = await _context.Products.Include("ProductCategory").Include("Brand").ToListAsync(),
            Pets = await _context.Pets.Include("PetCategory").ToListAsync()
        };
        var slider = await _context.Sliders.Where(s => s.Status == 1).ToListAsync();
        ViewBag.Slider = slider;
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> AddWishlist(int Id, WishlistModel wishlistmodel)
    {
        var user = await _userManager.GetUserAsync(User);

        var wishlistProduct = new WishlistModel
        {
            ProductId = Id,
            UserId = user!.Id,
        };

        _context.Wishlist.Add(wishlistProduct);
        try
        {
            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Add to wishlisht Successfully" });
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while adding to wishlist table.");
        }
    }

    public async Task<IActionResult> DeleteWishlist(int Id)
    {
        WishlistModel? wishlist = await _context.Wishlist.FindAsync(Id);
        if (wishlist == null)
        {
            return NotFound();
        }

        _context.Wishlist.Remove(wishlist);
        await _context.SaveChangesAsync();

        TempData["success"] = "Wishlist remove successfully";
        return RedirectToAction("Wishlist", "Home");
    }

    public async Task<IActionResult> Wishlist()
    {
        var wishlist_product = await (from w in _context.Wishlist
                                      join p in _context.Products on w.ProductId equals p.Id
                                      select new { Product = p, Wishlists = w })
                           .ToListAsync();
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
        var pets = await _context.Pets
            .Where(p => EF.Functions.Like(p.Name, $"%{searchString}%") || EF.Functions.Like(p.Description, $"%{searchString}%"))
            .ToListAsync();
        var products = await _context.Products
            .Where(p => EF.Functions.Like(p.Name, $"%{searchString}%") || EF.Functions.Like(p.Description, $"%{searchString}%"))
            .ToListAsync();
        if (pets.Count == 0 && products.Count == 0)
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
            return View("/Views/Shared/NotFoundPage");
        }
        else
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}