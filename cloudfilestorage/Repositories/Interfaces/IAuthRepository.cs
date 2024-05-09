using cloudfilestorage.Models;

namespace cloudfilestorage.Repositories.Interface;

public interface IAuthRepository
{
    public void Register(string login, string password);
    public string Login(string login, string password);
    public IEnumerable<User> FindByLoginAndPass(string login, string password);
}