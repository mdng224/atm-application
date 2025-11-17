using App.Application.Abstractions.Handlers;
using App.Application.Accounts.Queries.GetAccounts;
using App.Application.Common.Dtos;
using App.Application.Common.Results;
using Microsoft.Extensions.DependencyInjection;

namespace App.Application.Accounts;

public static class AccountsModule
{
    public static IServiceCollection AddAccountsApplication(this IServiceCollection services)
    {
        // Queries
        services.AddScoped<IQueryHandler<GetAccountsQuery, Result<List<AccountDto>>>, GetAccountsHandler>();
        
        return services;
    }
}