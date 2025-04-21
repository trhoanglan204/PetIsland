using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetIsland.DataAccess.Data;
using PetIsland.Models;
using PetIsland.Models.ViewModels;
using PetIslandWeb.Services.ORS;

#pragma warning disable IDE0290

namespace PetIslandWeb.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly GeocodingService _geoService;
    private static List<GroupMemberModel>? ListMembers;
    private readonly UserManager<AppUserModel> _userManager;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<AppUserModel> userManager, GeocodingService geoService)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _geoService = geoService;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new HomeViewModel
        {
            Products = await _context.Products.Include(p => p.ProductCategory).Include(p => p.Brand).ToListAsync(),
            Pets = await _context.Pets.Include(p => p.PetCategory).ToListAsync()
        };
        var slider = await _context.Sliders.Where(s => s.Status == 1).ToListAsync();
        ViewBag.Slider = slider;
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
        var searchResult = new SearchVM
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
                    Nickname = "anhnottham",
                    LinkFB = "https://www.facebook.com/anhnottham",
                    LinkLinkedin = "https://www.linkedin.com/in/anhnottham/"
                },
                new GroupMemberModel
                {
                    Name = "Nguyen Anh Khoi",
                    MSSV = "AT19N0121",
                    ImageUrl = "shin_uncensored.jpg",
                    Nickname = "anhkhoii",
                    LinkFB = "https://www.facebook.com/anhkhoii",
                    LinkLinkedin = "https://www.linkedin.com/in/anhkhoii/"
                },
                new GroupMemberModel
                {
                    Name = "Truong Hoang Lan",
                    MSSV = "AT19N0123",
                    ImageUrl = "shin_dev.jpg",
                    Nickname = "hlaan",
                    LinkFB = "https://www.facebook.com/hlaan.dev",
                    LinkLinkedin = "https://www.linkedin.com/in/hlaan/"
                },
                new GroupMemberModel
                {
                    Name = "Truong Van Thieu",
                    MSSV = "AT19N0138",
                    ImageUrl = "shin_sleep.jpg",
                    Nickname = "TvT",
                    LinkFB = "https://www.facebook.com/tvthieu",
                    LinkLinkedin = "https://www.linkedin.com/in/truongvanthieu/"
                }
            ];
        return View(ListMembers);
    }

    [HttpGet]
    public async Task<IActionResult> Contact()
    {
        var contact = await _context.Contact.FirstOrDefaultAsync();
        if (contact != null)
        {
            var key = contact.ORS_Key;
            if (key != null && (contact.ORS_lon == 0 || contact.ORS_lat == 0))
            {
                var ORS = await _geoService.GeocodeSearchAsync(contact.Address!);
                if (ORS != null)
                {
                    contact.ORS_lon = ORS.lon;
                    contact.ORS_lat = ORS.lat;
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
            }
        }
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