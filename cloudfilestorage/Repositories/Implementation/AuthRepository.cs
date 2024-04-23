using cloudfilestorage.Database;
using cloudfilestorage.Models;
using cloudfilestorage.Repositories.Interface;
using Dapper;

namespace cloudfilestorage.Repositories;

public class AuthRepository: IAuthRepository
{
    private readonly AppDbContext _context;
    
    public AuthRepository(AppDbContext context)
    {
        _context = context;
    }
    public void Register(string login, string password) 
    {
        using (var connection = _context.CreateConnection())
        {
            string query = "INSERT INTO [Users] (Login, Password) VALUES (@l, @p);";
            
            connection.Query(query, new { l = login, p = password });
        }
    }

    public string Login(string login, string password)
    {
        using (var connection = _context.CreateConnection())
        {
            string query = "SELECT [Login] FROM [Users] WHERE Login = @n and Password = @p;";
            
            return connection.QueryFirstOrDefault<string>(query, new { l = login, p = password});
        }
    }
    
    public IEnumerable<User> FindByLoginAndPass(string login, string password)
    {
        using (var connection = _context.CreateConnection())
        {
            return connection.Query<User>("SELECT * FROM [Users] WHERE Login = @l AND Password = @p", new { l = login, p = password });
        }
    }
}