using System.ComponentModel.DataAnnotations;

namespace cloudfilestorage.Models;

public class User
{
    public int ID { get; set; }
    [Required] public string Login { get; set; }
    [Required] public string Password { get; set; }
}