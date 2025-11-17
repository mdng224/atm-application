using App.Application.Abstractions.Persistence;
using App.Application.Abstractions.Persistence.Readers;
using App.Application.Abstractions.Persistence.Repositories;
using App.Infrastructure.Persistence;
using App.Infrastructure.Persistence.Readers;
using App.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace App.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        // --- Connection / DataSource ---
        var connectionString =
            config.GetConnectionString("appdb")
            ?? config["Aspire:Npgsql:ConnectionString"]
            ?? throw new InvalidOperationException(
                "No connection string found. Set ConnectionStrings:appdb or Aspire:Npgsql:ConnectionString.");

        var dataSource = new NpgsqlDataSourceBuilder(connectionString).Build();
        
        services.AddSingleton(dataSource);

        // --- Current user + audit interceptor (per-request) ---
        services.AddHttpContextAccessor();
        
        // --- DbContext (single registration) ---
        services.AddDbContext<AppDbContext>((sp, o) =>
        {
            o.UseNpgsql(dataSource, npgsql => npgsql.EnableRetryOnFailure());
        });

        // --- Repositories / Data access ---
        services.AddScoped<ITransactionReader, TransactionReader>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IAccountReader, AccountReader>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        return services;
    }
}
