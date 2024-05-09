using cloudfilestorage.Models;

namespace cloudfilestorage.Services.Interface;

public interface IAuthService
{
    public void Register(string login, string password);
    public string Login(string login, string password);
    public string? Authenticate(string login, string password);
    public User GetUser(string login, string password);
}