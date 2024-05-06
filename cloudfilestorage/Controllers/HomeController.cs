using Amazon.S3.Model;
using cloudfilestorage.Models;
using cloudfilestorage.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace cloudfilestorage.Controllers;

public class HomeController : Controller
{
    private readonly IFileStorageService _fileStorageService;
    
    public HomeController(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index(string folderName)
    {
        var foundFiles = await _fileStorageService.GetAllFiles(GetUsersStorage(), folderName);
        
        var viewModel = new IndexViewModel
        {
            AllFiles = foundFiles,
            FoundObject = null 
        };
        
        return View(viewModel);
    }
    
    public string GetUsersStorage()
    {
        return $"user-{HttpContext.Session.GetString("LoggedInUserID")}-files";
    }
}