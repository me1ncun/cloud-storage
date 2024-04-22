using Microsoft.Data.SqlClient;

namespace cloudfilestorage.Database;

public class AppDbContext
{
    public static SqlConnection CreateConnection()
    {
        return new SqlConnection("Data Source=.;Initial Catalog=FileStorage;Integrated Security=true;");
    }
}