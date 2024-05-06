using Amazon.S3.Model;

namespace cloudfilestorage.Utils;

public static class FileUtility
{
    public static ListObjectsV2Response SortFiles(ListObjectsV2Response files, string folderName)
    {
        var result = new ListObjectsV2Response();
        int prefixLength = folderName.Length;
        foreach (var obj in files.S3Objects)
        {
            if (obj.Key.Substring(prefixLength).Trim('/').Split('/').Length == 1  && obj.Key != folderName)
            {
                result.S3Objects.Add(obj);
            }
        }            
        return result;
    }
}