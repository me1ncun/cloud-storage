using Amazon.S3;
using Amazon.S3.Model;
using cloudfilestorage.Services.Interface;
using cloudfilestorage.Utils;

namespace cloudfilestorage.Services.Implementation;

public class FileStorageService : IFileStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _standartBucketName = "user-files-aws";

    public FileStorageService(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }

    public async Task<bool> UploadFiles(IFormFile file, string path, string usersFolder)
    {
        try
        {
            PutObjectRequest objectRequest;

            if (path != null)
            {
                objectRequest = new PutObjectRequest()
                {
                    BucketName = _standartBucketName,
                    Key = $"{path}/{file.FileName}"
                };
            }
            else
            {
                objectRequest = new PutObjectRequest()
                {
                    BucketName = _standartBucketName,
                    Key = $"{usersFolder}/{file.FileName}"
                };
            }

            var response = await _s3Client.PutObjectAsync(objectRequest);

            return true;
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public async Task<bool> CreateBucket(string bucketName, string path, string usersFolder)
    {
        try
        {
            PutObjectRequest request;
            if (path != null)
            {
                request = new PutObjectRequest
                {
                    BucketName = _standartBucketName,
                    Key = $"{path}/{bucketName}/",
                    ContentBody = ""
                };
            }
            else
            {
                request = new PutObjectRequest
                {
                    BucketName = _standartBucketName,
                    Key = $"{usersFolder}/{bucketName}/",
                    ContentBody = ""
                };
            }

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
        try
        {
            if (folderName != null)
            {
                var request = new ListObjectsV2Request()
                {
                    BucketName = $"{_standartBucketName}",
                    Prefix = $"{folderName}",
                };
                var response = await _s3Client.ListObjectsV2Async(request);
                return FileUtility.GetFiles(response, folderName);
            }
            else
            {
                var request = new ListObjectsV2Request()
                {
                    BucketName = $"{_standartBucketName}",
                    Prefix = $"{bucketName}",
                };

                var response = await _s3Client.ListObjectsV2Async(request);
                return FileUtility.GetFiles(response, bucketName);
            }
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
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

            await _s3Client.DeleteObjectAsync(deleteObjectRequest);

            return true;
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine(e.Message);
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
            Console.WriteLine(e.Message);
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
    
    public async Task<bool> RenameObject(string oldName, string newName, string path, string usersFolder)
    {
        try
        {
            CopyObjectRequest copyObjectRequest;
            if (path != null)
            {
                copyObjectRequest = new CopyObjectRequest
                {
                    SourceBucket = _standartBucketName,
                    SourceKey = $"{oldName}",
                    DestinationBucket = _standartBucketName,
                    DestinationKey = $"{path}/{newName}",
                };
            }
            else
            {
                copyObjectRequest = new CopyObjectRequest
                {
                    SourceBucket = _standartBucketName,
                    SourceKey = $"{oldName}",
                    DestinationBucket = _standartBucketName,
                    DestinationKey = $"{usersFolder}/{newName}",
                };
            }
            
            await _s3Client.CopyObjectAsync(copyObjectRequest);
            await DeleteObject(oldName);
            return true;
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public async Task<bool> RenameBucket(string oldName, string newName, string path, string usersFolder)
    {
        try
        {
            await DeleteObject(oldName);
            await CreateBucket(newName, path, usersFolder);
            return true;
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }
}