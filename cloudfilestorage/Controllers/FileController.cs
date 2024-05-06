using Amazon.S3;
using Amazon.S3.Model;
using cloudfilestorage.Models;
using cloudfilestorage.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace cloudfilestorage.Controllers;

public class FileController : Controller
{
    private readonly IFileStorageService _fileStorageService;
    public FileController(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }
    
    [HttpGet]
    public IActionResult Error() => View();
    
    [HttpPost]
    public async Task<IActionResult> UploadFiles(IFormFile file)
    {
        var upload = _fileStorageService.UploadFiles(file);

        if (upload.Result == false)
        {
            return RedirectToAction("Error", "File");
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }


    [HttpPost]
    public async Task<IActionResult> CreateBucket(string bucketName)
    {
        var createdBucket = _fileStorageService.CreateBucket(bucketName);
        
        if (createdBucket.Result == false)
        {
            return RedirectToAction("Error", "File");
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }
    
    [HttpPost]
    public IActionResult TransportFolderName(string folderName)
    {
        return RedirectToAction("Index", "Home", new { folderName = folderName });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteObject(string fileName)
    {
        var deletedObject = _fileStorageService.DeleteObject(fileName);
        
        if (deletedObject.Result == false)
        {
            return RedirectToAction("Error", "File");
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> RenameObject(string oldName, string newName)
    {
        var renamedObject = _fileStorageService.RenameObject(oldName, newName);
        
        if (renamedObject.Result == false)
        {
            return RedirectToAction("Error", "File");
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> RenameBucket(string oldName, string newName)
    {
        var renamedBucket = _fileStorageService.RenameBucket(oldName, newName);
        
        if (renamedBucket.Result == false)
        {
            return RedirectToAction("Error", "File");
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> DownloadObject(string fileName)
    {
        var downloadedObject = _fileStorageService.DownloadObject(fileName);

        if (downloadedObject.Result == false)
        {
            return RedirectToAction("Error", "File");
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    /*[HttpPost]
    public async Task<IActionResult> SearchedObject(string searchedObj)
    {
        var allFiles = _fileStorageService.GetAllFiles()
        var foundObject = allFiles.S3Objects.FirstOrDefault(x => x.Key.Contains(searchedObj));

        if (foundObject != null)
        {
            var viewModel = new IndexViewModel
            {
                AllFiles = allFiles,
                FoundObject = foundObject
            };

            return RedirectToAction("Index", "File", viewModel);
        }
        else
        {
            return RedirectToAction("Error", "File");
        }
    }*/

    public async Task<IActionResult> OpenFolder(string folderName)
    {
        return View();
    }
    
    public string GetUsersStorage()
    {
        return $"user-{HttpContext.Session.GetString("LoggedInUserID")}-files";
    }
}