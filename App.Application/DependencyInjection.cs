using App.Application.Accounts;
using App.Application.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace App.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Modules
        services.AddAccountsApplication();
        services.AddTransactionsApplication();

        return services;
    }
}