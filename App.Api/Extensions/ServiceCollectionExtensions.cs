using App.Application;
using App.Infrastructure;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace App.Api.Extensions;

public static class ServiceCollectionExtensions
{
    private const string CorsPolicy = "frontend";

    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration cfg)
    {
        // --- CORS (Vite dev) -----------------------------------------------
        services.AddCors(o => o.AddPolicy(CorsPolicy, p =>
            p.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")
                .AllowAnyHeader()
                .AllowAnyMethod()
        ));

        // --- JSON naming policy (camelCase globally) ------------------------
        services.ConfigureHttpJsonOptions(jo =>
        {
            jo.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            jo.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        // --- OpenAPI & app layers -------------------------------------------
        services.AddOpenApi();
        services.AddApplication();
        services.AddInfrastructure(cfg);

        return services;
    }

    internal static string CorsPolicyName() => CorsPolicy;
}