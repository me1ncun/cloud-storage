using System.Data;
using Microsoft.Data.SqlClient;

namespace cloudfilestorage.Database;

public class AppDbContext
{
    private readonly IConfiguration _configuration;
    private readonly string connectionstring;
    public AppDbContext(IConfiguration configuration) {
        this._configuration = configuration;
        this.connectionstring = this._configuration.GetConnectionString("DefaultConnection");
    }
    public IDbConnection CreateConnection() => new SqlConnection(connectionstring);
}