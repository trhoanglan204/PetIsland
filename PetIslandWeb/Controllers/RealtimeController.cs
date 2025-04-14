using Microsoft.AspNetCore.Mvc;

namespace PetIslandWeb.Controllers;

public class RealtimeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
