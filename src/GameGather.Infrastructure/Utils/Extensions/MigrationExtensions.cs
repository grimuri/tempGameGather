using GameGather.Infrastructure.Persistance;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameGather.Infrastructure.Utils.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigration(this WebApplication app)
    {
        using var scope = app
            .Services.
            CreateScope();
        var dbContext = scope
            .ServiceProvider
            .GetRequiredService<GameGatherDbContext>();
        dbContext.Database.Migrate();
    }
}