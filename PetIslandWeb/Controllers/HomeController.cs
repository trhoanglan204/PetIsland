using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetIsland.DataAccess.Data;
using PetIsland.Models;
using PetIsland.Models.ViewModels;

#pragma warning disable IDE0290

namespace PetIslandWeb.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
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
        var products = await _context.Products.Include(p => p.ProductCategory).Include(p => p.Brand).ToListAsync();
        var pets = await _context.Pets.Include(p => p.PetCategory).ToListAsync();
        bool morepet = false;
        bool moreproduct = false;
        if (pets.Count > 9)
        {
            pets = pets.Take(9).ToList();
            morepet = true;

        }
        if (products.Count > 9)
        {
            products = products.Take(9).ToList();
            moreproduct = true;
        }
        var viewModel = new HomeViewModel
        {
            Products = products,
            Pets = pets,
            MorePet = morepet,
            MoreProduct = moreproduct
        };
        var slider = await _context.Sliders.Where(s => s.Status == 1).ToListAsync();
        ViewBag.Sliders = slider;
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> AddWishlist(int Id)
    {
        WishlistModel? wishlist = await _context.Wishlist.FindAsync(Id);
        if (wishlist != null)
        {
            //ignore
            return Ok(new { success = true, message = "Add to wishlisht Successfully" });
        }
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
    public IActionResult Search()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Search(string? searchString)
    {
        if (string.IsNullOrEmpty(searchString))
        {
            return RedirectToAction("Index");
        }
        var pets = await _context.Pets
            .Where(p => EF.Functions.Like(p.Name, $"%{searchString}%") || EF.Functions.Like(p.Description, $"%{searchString}%"))
            .ToListAsync();
        var products = await _context.Products
            .Where(p => EF.Functions.Like(p.Name, $"%{searchString}%") || EF.Functions.Like(p.Description, $"%{searchString}%"))
            .ToListAsync();
        ViewBag.KeyWord = searchString;
        if (pets.Count == 0 && products.Count == 0)
        {
            return View(null);
        }
        var searchResult = new SearchVM
        {
            Products = products,
            Pets = pets
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
                    Nickname = "anhnottham",
                    LinkFB = "https://www.facebook.com/share/1A1qQNHU31/",
                    LinkLinkedin = "https://www.linkedin.com/in/anh-nguyen-92a56b219/"
                },
                new GroupMemberModel
                {
                    Name = "Nguyen Anh Khoi",
                    MSSV = "AT19N0121",
                    ImageUrl = "shin_uncensored.jpg",
                    Nickname = "anhkhoii",
                    LinkFB = "https://www.facebook.com/share/1FoMp4Cp8t/",
                    LinkLinkedin = "https://www.linkedin.com/in/khoi-nguyen-498255260/"
                },
                new GroupMemberModel
                {
                    Name = "Truong Hoang Lan",
                    MSSV = "AT19N0123",
                    ImageUrl = "shin_dev.jpg",
                    Nickname = "hlaan",
                    LinkFB = "https://www.facebook.com/lan.truonghoang.31",
                    LinkLinkedin = "https://www.linkedin.com/in/trhoanglan04/"
                },
                new GroupMemberModel
                {
                    Name = "Truong Van Thieu",
                    MSSV = "AT19N0138",
                    ImageUrl = "shin_sleep.jpg",
                    Nickname = "TvT",
                    LinkFB = "https://www.facebook.com/share/1AJHrqmk5a/",
                    LinkLinkedin = "https://www.linkedin.com/in/tr%C6%B0%C6%A1ng-v%C4%83n-thi%E1%BB%87u-b01a15345/"
                }
            ];
        return View(ListMembers);
    }

    [HttpGet]
    public async Task<IActionResult> Contact()
    {
        var contact = await _context.Contact.FirstOrDefaultAsync();
        return View(contact); //nullable
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int? statuscode = null)
    {
        if (statuscode == StatusCodes.Status404NotFound)
        {
            return View("~/Views/Shared/NotFoundPage.cshtml");
        }
        else if (statuscode == StatusCodes.Status403Forbidden)
        {
            return View("~/Views/Account/AccessDenied.cshtml");
        }
        else if (statuscode == StatusCodes.Status500InternalServerError)
        {
            return View("~/Views/Shared/InternalServerError.cshtml");
        }
        else
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}