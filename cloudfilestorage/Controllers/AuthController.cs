using System.Diagnostics;
using cloudfilestorage.Models;
using cloudfilestorage.Services.Implementation;
using cloudfilestorage.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace cloudfilestorage.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public IActionResult Login(UserViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = _authService.GetUser(model.Login, model.Password);
            if (user != null)
            {
                HttpContext.Session.SetString("LoggedInUser", user.Login); 
                HttpContext.Session.SetString("LoggedInUserID", Convert.ToString(user.ID));
                
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("notCorrect", "Неправильный логин или пароль. Попробуйте еще раз.");
            }
        }

        return View();
    }

    [HttpPost]
    public IActionResult Register(UserViewModel model)
    {
        if (ModelState.IsValid)
        {
            _authService.Register(model.Login, model.Password);
            var user = _authService.GetUser(model.Login, model.Password);
            if (user != null)
            {
                return View(user);
            }
        }

        return View();
    }

    [HttpGet]
    public IActionResult Logout()
    {
        try
        {
            HttpContext.Session.Clear();
            HttpContext.Response.Clear();

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}