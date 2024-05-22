using cloudfilestorage.Database;
using Microsoft.EntityFrameworkCore;

namespace tennis.Extensions;

public static class MigrationExtension
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<AppDbContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
    }
}