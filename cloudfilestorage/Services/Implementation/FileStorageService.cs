using Amazon.S3;
using Amazon.S3.Model;
using cloudfilestorage.Services.Interface;
using cloudfilestorage.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cloudfilestorage.Services.Implementation;

public class FileStorageService : IFileStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _standartBucketName = "user-files-aws";

    public FileStorageService(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }

    public async Task<bool> UploadFiles(IFormFile file)
    {
        try
        {
            var bucketExists = await _s3Client.DoesS3BucketExistAsync("user-files-aws");
            if (!bucketExists)
            {
                var bucketRequest = new PutBucketRequest
                {
                    BucketName = _standartBucketName,
                    UseClientRegion = true
                };
                await _s3Client.PutBucketAsync(bucketRequest);
            }

            var objectRequest = new PutObjectRequest()
            {
                BucketName = _standartBucketName,
                Key = $"{file.FileName}"
            };
            var response = await _s3Client.PutObjectAsync(objectRequest);

            return true;
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }
    
    public async Task<bool> CreateBucket(string bucketName)
    {
        try
        {
            var request = new PutObjectRequest
            {
                BucketName = _standartBucketName,
                Key = $"{bucketName}/",
                ContentBody = ""
            };
            await _s3Client.PutObjectAsync(request);

            return true;
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }
    
    public async Task<ListObjectsV2Response> GetAllFiles(string bucketName, string folderName)
    {
        if (folderName != null)
        {
            var request = new ListObjectsV2Request()
            {
                BucketName = $"{_standartBucketName}",
                Prefix = $"{folderName}",
            };
            var response = await _s3Client.ListObjectsV2Async(request);
            return FileUtility.SortFiles(response, folderName);
        }
        else
        {
            var request = new ListObjectsV2Request()
            {
                BucketName = $"{_standartBucketName}",
                Prefix = $"{bucketName}",
            };
        
            var response = await _s3Client.ListObjectsV2Async(request);
            return FileUtility.SortFiles(response, bucketName);
        }
    }
    
    public async Task<bool> DeleteObject(string fileName)
    {
        try
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = _standartBucketName,
                Key = fileName
            };

            Console.WriteLine("Deleting an object");
            await _s3Client.DeleteObjectAsync(deleteObjectRequest);

            return true;
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine("Error encountered on server. Message:'{0}' when deleting an object", e.Message);
            return false;
        }
    }
    
    public async Task<bool> DownloadObject(string fileName)
    {
        try
        {
            var request = new GetObjectRequest
            {
                BucketName = _standartBucketName,
                Key = fileName
            };
            using (GetObjectResponse response = await _s3Client.GetObjectAsync(request))
            {
                string dest = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
                await response.WriteResponseStreamToFileAsync(dest, true, CancellationToken.None);
            }
            
            return true;
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
    
    public async Task<bool> RenameObject(string oldName, string newName)
    {
        try
        {
            var copyObjectRequest = new CopyObjectRequest
            {
                SourceBucket =_standartBucketName,
                SourceKey = oldName,
                DestinationBucket = _standartBucketName,
                DestinationKey = newName
            };
            await _s3Client.CopyObjectAsync(copyObjectRequest);
            await DeleteObject(oldName);
            return true;
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
    
    public async Task<bool> RenameBucket(string oldName, string newName)
    {
        try
        {
            await DeleteObject(oldName);
            await CreateBucket(newName);
            return true;
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
    
    public async Task CreateUsersBucket(int userId)
    {
        try
        {
            var request = new PutObjectRequest
            {
                BucketName = _standartBucketName,
                Key = $"user-{userId}-files/",
                ContentBody = ""
            };
            await _s3Client.PutObjectAsync(request);
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}