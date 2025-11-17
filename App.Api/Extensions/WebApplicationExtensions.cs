using App.Infrastructure.Persistence;
using App.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace App.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseApi(this WebApplication app)
    {
        // dev-only swagger
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("/openapi/v1.json", "ATM API v1");
            });
        }
        else
        {
            app.UseHttpsRedirection();
        }

        app.UseCors(ServiceCollectionExtensions.CorsPolicyName());

        // health
        app.MapGet("/health/db", async (NpgsqlDataSource ds) =>
        {
            await using var cmd = ds.CreateCommand("SELECT 1");
            var result = await cmd.ExecuteScalarAsync();
            return Results.Ok(new { db = result });
        });

        app.MapAtmEndpoints();

        return app;
    }

    // Ensure database is created/migrated
    public static async Task MigrateDatabaseAsync<TContext>(this WebApplication app)
        where TContext : DbContext
    {
        await using var scope = app.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<TContext>();

        try
        {
            await db.Database.MigrateAsync();

            if (db is AppDbContext appDb)
                await DbSeeder.SeedAsync(appDb);
        }
        catch (Exception ex)
        {
            var logger = app.Logger;
            logger.LogError(ex, "Database migration/seed failed");
            throw;
        }
    }
}