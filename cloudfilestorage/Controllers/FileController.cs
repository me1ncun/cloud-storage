using Microsoft.AspNetCore.Mvc;

namespace cloudfilestorage.Controllers;

public class FileController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Upload()
    {
        return View();
    }
    
    public IActionResult Download()
    {
        return View();
    }
    
}