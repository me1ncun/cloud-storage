using System.Text.Json;
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
    public async Task<IActionResult> RenameObject(string oldName, string newName, string path)
    {
        var renamedObject = _fileStorageService.RenameObject(oldName, newName, path, GetUsersStorage());
        
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
    public async Task<IActionResult> RenameBucket(string oldName, string newName, string path)
    {
        var renamedBucket = _fileStorageService.RenameBucket(oldName, newName, path, GetUsersStorage());
        
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
    public async Task<IActionResult> Search(string searchedObj)
    {
        if (!string.IsNullOrEmpty(searchedObj))
        {
            var allFiles = await _fileStorageService.GetAllFiles(GetUsersStorage(), null);
            var foundObjects = allFiles.S3Objects.Where(x => x.Key.Contains(searchedObj)).ToList();
            
            var serializedObjects = JsonSerializer.Serialize(foundObjects);
            
            return RedirectToAction("Search", "Home", new {foundObjects = serializedObjects });
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> UploadFiles(IFormFile file, string path)
    {
        var upload = _fileStorageService.UploadFiles(file, path, GetUsersStorage());

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
    public async Task<IActionResult> CreateBucket(string bucketName, string path)
    {
        var createdBucket = _fileStorageService.CreateBucket(bucketName, path, GetUsersStorage());
        
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
    public IActionResult TransportFolderName(string folderName)
    {
        return RedirectToAction("Index", "Home", new { folderName = folderName });
    }
    
    [HttpPost]
    public IActionResult FilePage(string fileName, long fileSize, DateTime fileLastModified)
    {
        S3Object file = new S3Object
        {
            Key = fileName,
            Size = fileSize,
            LastModified = fileLastModified
        };
        
        var serializedFile = JsonSerializer.Serialize(file);
        
        return RedirectToAction("FilePage", "Home", new { file = serializedFile });
    }
    
    public string GetUsersStorage()
    {
        return $"user-{HttpContext.Session.GetString("LoggedInUserID")}-files";
    }
}