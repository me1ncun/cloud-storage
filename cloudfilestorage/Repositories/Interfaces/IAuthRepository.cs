using cloudfilestorage.Models;

namespace cloudfilestorage.Repositories.Interface;

public interface IAuthRepository
{
    public void Register(string login, string password);
    public  Task<string> Login(string login, string password);
    public Task<User> FindByLoginAndPass(string login, string password);
}