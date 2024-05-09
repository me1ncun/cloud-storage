using Amazon.S3.Model;
using cloudfilestorage.Models;
using cloudfilestorage.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace cloudfilestorage.Controllers;

public class HomeController : Controller
{
    private readonly IFileStorageService _fileStorageService;
    
    public HomeController(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index(string folderName, string foundObjects)
    {
        var foundObjectsList = !string.IsNullOrEmpty(foundObjects) ? JsonSerializer.Deserialize<List<S3Object>>(foundObjects) : null;

        var foundFiles = await _fileStorageService.GetAllFiles(GetUsersStorage(), folderName);
        var path = HttpContext.Request.Query["folderName"];

        var viewModel = new IndexViewModel
        {
            AllFiles = foundFiles,
            FoundObjects = foundObjectsList,
            Path = path,
        };

        return View(viewModel);
    }

    [HttpGet]
    public IActionResult FilePage(S3Object fileName)
    {
        return View(fileName);
    }
    
    public string GetUsersStorage()
    {
        return $"user-{HttpContext.Session.GetString("LoggedInUserID")}-files";
    }
}