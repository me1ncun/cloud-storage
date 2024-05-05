namespace cloudfilestorage.Models;

public class AWSCredentials
{
    public const string Position = "AWSConfiguration";
    public string AWSAccessKey { get; set; } = String.Empty;
    public string AWSSecretKey { get; set; } = String.Empty;
    
}