using Amazon.S3.Model;
using cloudfilestorage.Models;

namespace cloudfilestorage.Services.Interface;

public interface IFileStorageService
{
    public Task<bool> UploadFiles(IFormFile file);
    public Task<bool> CreateBucket(string bucketName);
    public Task<ListObjectsV2Response> GetAllFiles(string bucketName, string folderName);
    public Task<bool> DeleteObject(string fileName);
    public Task<bool> RenameObject(string oldName, string newName);
    public Task<bool> RenameBucket(string oldName, string newName);
    public Task<bool> DownloadObject(string fileName);
    public Task CreateUsersBucket(int userId);

}