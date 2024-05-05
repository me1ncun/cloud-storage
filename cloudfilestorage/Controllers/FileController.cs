using Amazon.S3;
using Amazon.S3.Model;
using cloudfilestorage.DTO;
using cloudfilestorage.Models;
using cloudfilestorage.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using S3Object = Amazon.S3.Model.S3Object;
using ObjectS3 = cloudfilestorage.Models.ObjectS3;

namespace cloudfilestorage.Controllers;

public class FileController : Controller
{
    private IConfiguration _configuration;
    private readonly IStorageService _storageService;
    private readonly IAmazonS3 _s3Client;
    private readonly IConfiguration Configuration;
    private readonly AWSCredentials _awsCredentials;

    public FileController(IStorageService storageService, IConfiguration configuration, IAmazonS3 s3Client)
    {
        _storageService = storageService;
        Configuration = configuration;
        _awsCredentials = new AWSCredentials();
        Configuration.GetSection(AWSCredentials.Position).Bind(_awsCredentials);
        _s3Client = s3Client;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var request = new ListObjectsV2Request()
        {
            BucketName = "user-files-aws",
            Prefix = ""
        };
        
        var response = await _s3Client.ListObjectsV2Async(request);
        return View(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> UploadFiles(IFormFile file)
    {
        var bucketExists = await _s3Client.DoesS3BucketExistAsync("user-files-aws");
        if(!bucketExists)
        {
           var bucketRequest = new PutBucketRequest
           {
               BucketName = "user-files-aws",
               UseClientRegion = true
           };
           await _s3Client.PutBucketAsync(bucketRequest);
        }

        var objectRequest = new PutObjectRequest()
        {
            BucketName = "user-files-aws",
            Key = $"{file.FileName}"
        };
        var response = await _s3Client.PutObjectAsync(objectRequest);

        return RedirectToAction("Index", "File");
    }


    [HttpPost]
    public async Task<IActionResult> CreateBucket(string bucketName)
    {
        var standardBucketName = $"user-files-aws";
        
            var request = new PutObjectRequest
            {
                BucketName = standardBucketName,
                Key = $"{bucketName}/",
                ContentBody = ""
            };
            await _s3Client.PutObjectAsync(request);

        return RedirectToAction("Index", "File");
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllFiles()
    {
        var request = new ListObjectsV2Request()
        {
            BucketName = "user-files-aws",
            Prefix = ""
        };
        
        var response = await _s3Client.ListObjectsV2Async(request);
        return View(response);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteObject(string fileName)
    {
        try
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = "user-files-aws",
                Key = fileName
            };

            Console.WriteLine("Deleting an object");
            await _s3Client.DeleteObjectAsync(deleteObjectRequest);
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine("Error encountered on server. Message:'{0}' when deleting an object", e.Message);
        }
        return RedirectToAction("Index", "File");
    }

    [HttpPost]
    public async Task<IActionResult> RenameObject(string oldName, string newName)
    {
        try
        {
            var copyObjectRequest = new CopyObjectRequest
            {
                SourceBucket = "user-files-aws",
                SourceKey = oldName,
                DestinationBucket = "user-files-aws",
                DestinationKey = newName
            };
            await _s3Client.CopyObjectAsync(copyObjectRequest);
            await DeleteObject(oldName);
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return RedirectToAction("Index", "File");
    }
    
    [HttpPost]
    public async Task<IActionResult> RenameBucket(string oldName, string newName)
    {
        try
        {
            await DeleteObject(oldName);
            await CreateBucket(newName);
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return RedirectToAction("Index", "File");
    }
    
    [HttpPost]
    public async Task<IActionResult> DownloadObject(string fileName)
    {
        var request = new GetObjectRequest
        {
            BucketName = "user-files-aws",
            Key = fileName
        };
        using (GetObjectResponse response = await _s3Client.GetObjectAsync(request))
        {
            string dest = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
            await response.WriteResponseStreamToFileAsync(dest, true, CancellationToken.None);
        }
        
        return RedirectToAction("Index", "File");
    }

    public async Task<IActionResult> SearchObject(string searchedObj)
    {
        var result =  GetAllFiles();
        
        return View();
    }
}