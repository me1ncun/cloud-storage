using Amazon.S3.Model;

namespace cloudfilestorage.Models;

public class IndexViewModel
{
    public ListObjectsV2Response AllFiles { get; set; }
    public List<S3Object> FoundObjects { get; set; }
    public string Path { get; set; }
    
    public IndexViewModel(string path, List<S3Object> FoundObjects)
    {
        this.FoundObjects = FoundObjects;
        Path = GetCorrectPath(path);
    }
    public IndexViewModel()
    {
    }

    public static string GetCorrectPath(string fullPath)
    {
        var lastIndex = fullPath.LastIndexOf("/");
        var underLastIndex = fullPath.LastIndexOf("/", lastIndex);
        var result = fullPath.Substring(0, underLastIndex);
        return result;
    }
}