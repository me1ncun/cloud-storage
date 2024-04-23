using System.ComponentModel.DataAnnotations;

namespace cloudfilestorage.Models;

public class User
{
    [Required]
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }
}