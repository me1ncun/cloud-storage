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
    public async Task<IActionResult> Index(string folderName)
    {
        var viewModel = await GetAllFiles(folderName);
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Search(string foundObjects)
    {
        var foundObjectsList = !string.IsNullOrEmpty(foundObjects)
            ? JsonSerializer.Deserialize<List<S3Object>>(foundObjects)
            : null;
        var viewModel = new IndexViewModel
        {
            FoundObjects = foundObjectsList,
        };

        return View(viewModel);
    }
    
    public async Task<IndexViewModel> GetAllFiles(string folderName)
    {
        var foundFiles = await _fileStorageService.GetAllFiles(GetUsersStorage(), folderName);
        var path = HttpContext.Request.Query["folderName"];

        var viewModel = new IndexViewModel
        {
            AllFiles = foundFiles,
            Path = path,
        };

        return viewModel;
    }

    [HttpGet]
    public IActionResult FilePage(string file)
    {
        var desiredFile = JsonSerializer.Deserialize<S3Object>(file);
        return View(desiredFile);
    }
    
    public string GetUsersStorage()
    {
        return $"user-{HttpContext.Session.GetString("LoggedInUserID")}-files";
    }
}