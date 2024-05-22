using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cloudfilestorage.Models;
[Table("users")]
public class User
{
    [Column("id")]
    public int ID { get; set; }
    [Column("login")]
    [Required] 
    public string Login { get; set; }
    [Column("password")]
    [Required] 
    public string Password { get; set; }
}