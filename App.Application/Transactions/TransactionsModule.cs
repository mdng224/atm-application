using App.Application.Abstractions.Handlers;
using App.Application.Common.Dtos;
using App.Application.Common.Pagination;
using App.Application.Common.Results;
using App.Application.Transactions.Commands.Deposit;
using App.Application.Transactions.Commands.Transfer;
using App.Application.Transactions.Commands.Withdraw;
using App.Application.Transactions.Queries.GetTransactions;
using Microsoft.Extensions.DependencyInjection;

namespace App.Application.Transactions;

public static class TransactionsModule
{
    public static IServiceCollection AddTransactionsApplication(this IServiceCollection services)
    {
        // Queries
        services.AddScoped<IQueryHandler<GetTransactionsQuery, Result<PagedResult<TransactionDto>>>, GetTransactionsHandler>();

        // Commands
        services.AddScoped<ICommandHandler<DepositCommand, Result<Unit>>, DepositHandler>();
        services.AddScoped<ICommandHandler<WithdrawCommand, Result<Unit>>, WithdrawHandler>();
        services.AddScoped<ICommandHandler<TransferCommand, Result<Unit>>, TransferHandler>();
        
        return services;
    }
}