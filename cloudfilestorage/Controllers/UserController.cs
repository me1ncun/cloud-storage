using Microsoft.AspNetCore.Mvc;

namespace cloudfilestorage.Controllers;

public class UserController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Login()
    {
        return View();
    }
    
    public IActionResult Register()
    {
        return View();
    }
}