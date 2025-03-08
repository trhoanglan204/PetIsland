using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace PetIsland.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Search()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Search(string searchString)
    {
        var pets = await _context.Pets
            .Where(p => EF.Functions.Like(p.Name, $"%{searchString}%"))
            .ToListAsync();
        var products = await _context.Products
            .Where(p => EF.Functions.Like(p.Name, $"%{searchString}%"))
            .ToListAsync();
        if (pets.Count == 0 && products.Count == 0)
        {
            return NotFound();
        }
        var searchResults = new SearchViewModel
        {
            LPets = pets,
            LProducts = products,
            SearchKey = searchString
        };
        return View(searchResults);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}