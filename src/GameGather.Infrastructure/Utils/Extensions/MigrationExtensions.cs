using GameGather.Infrastructure.Persistance;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameGather.Infrastructure.Utils.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GameGatherDbContext>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var connectionString = configuration.GetConnectionString("Default");

        try
        {
            context.Database.Migrate();
        }
        catch (NpgsqlException ex)
        {
            var error = $"""
                Nie można połączyć się z bazą danych PostgreSQL.
                Connection string: '{connectionString}'
                Error: {ex.Message}
                """;
            
            app.Logger.LogError(error);
            throw new Exception(error, ex);
        }
    }
}