using System.Data;
using cloudfilestorage.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace cloudfilestorage.Database;

public class AppDbContext : DbContext
{
    private IConfiguration Configuration;

    public AppDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Configuration.GetConnectionString("Database"));
    }
    
    public DbSet<User> Users { get; set; }
}