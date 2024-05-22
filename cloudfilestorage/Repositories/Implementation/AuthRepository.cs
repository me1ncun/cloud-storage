using cloudfilestorage.Database;
using cloudfilestorage.Models;
using cloudfilestorage.Repositories.Interface;
using Dapper;
using Npgsql;

namespace cloudfilestorage.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly IConfiguration _configuration;
    private readonly string sqlString;
    public AuthRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        sqlString = _configuration.GetConnectionString("Database");
    }

    public async void Register(string login, string password)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(sqlString))
        {
            string query = """INSERT INTO users(login, password) VALUES (@login, @password)""";

            await connection.QueryAsync(query, new { login, password });
        }
    }

    public async Task<string> Login(string login, string password)
    {
            using (NpgsqlConnection connection = new NpgsqlConnection(sqlString))
            {
                string query = """SELECT (login) FROM users WHERE (login) = @login AND (password) = @password""";

                return await connection.QueryFirstAsync<string>(query, new { login, password });
            }
        
    }

    public async Task<User> FindByLoginAndPass(string login, string password)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(sqlString))
        {
            string query = """SELECT * FROM users WHERE (login) = @login AND (password) = @password""";

            return await connection.QueryFirstAsync<User>(query, new { login, password });
        }
    }
}