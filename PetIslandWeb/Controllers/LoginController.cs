using Microsoft.AspNetCore.Mvc;

namespace PetIslandWeb.Controllers;

public class LoginController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
