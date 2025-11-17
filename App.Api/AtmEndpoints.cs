using App.Api.Common;
using App.Api.Contracts;
using App.Application.Abstractions.Handlers;
using App.Application.Accounts.Queries.GetAccounts;
using App.Application.Common.Dtos;
using App.Application.Common.Pagination;
using App.Application.Common.Results;
using App.Application.Transactions.Commands.Deposit;
using App.Application.Transactions.Commands.Transfer;
using App.Application.Transactions.Commands.Withdraw;
using App.Application.Transactions.Queries.GetTransactions;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.Results;

namespace App.Api;

public static class AtmEndpoints
{
    public static void MapAtmEndpoints(this IEndpointRouteBuilder app)
    {
        var accounts = app.MapGroup("/accounts")
            .WithTags("Accounts");

        // GET /accounts
        accounts.MapGet("", HandleGetAccounts)
            .WithSummary("List all accounts");

        // GET /accounts/{id}/transactions
        accounts.MapGet("/{id:guid}/transactions", HandleGetTransactions)
            .WithSummary("Get transactions for an account");

        // POST /accounts/{id}/deposit
        accounts.MapPost("/{id:guid}/deposit", HandleDeposit)
            .WithSummary("Deposit funds into an account");

        // POST /accounts/{id:guid}/withdraw
        accounts.MapPost("/{id:guid}/withdraw", HandleWithdraw)
            .WithSummary("Withdraw funds from an account");

        // POST /accounts/transfer
        accounts.MapPost("/transfer", HandleTransfer)
            .WithSummary("Transfer funds between accounts");
    }

    private static async Task<IResult> HandleGetAccounts(
        [FromServices] IQueryHandler<GetAccountsQuery, Result<List<AccountDto>>> handler,
        CancellationToken ct)
    {
        var query = new GetAccountsQuery();
        var result = await handler.Handle(query, ct);
        
        return result.ToHttpResult(Ok);
    }

    private static async Task<IResult> HandleGetTransactions(
        [FromRoute] Guid id,
        [AsParameters] GetTransactionsRequest request,
        [FromServices] IQueryHandler<GetTransactionsQuery, Result<PagedResult<TransactionDto>>> handler,
        CancellationToken ct)
    {
        var query = new GetTransactionsQuery(id, new PagedQuery(request.Page, request.PageSize));
        var result = await handler.Handle(query, ct);

        return result.ToHttpResult(Ok);
    }

    private static async Task<IResult> HandleDeposit(
        [FromRoute] Guid id,
        [FromBody] DepositRequest request,
        [FromServices] ICommandHandler<DepositCommand, Result<Unit>> handler,
        CancellationToken ct)
    {
        var command = new DepositCommand(id, request.Amount);
        var result = await handler.Handle(command, ct);
        
        return result.ToHttpResult(Ok);
    }

    private static async Task<IResult> HandleWithdraw(
        [FromRoute] Guid id,
        [FromBody] WithdrawRequest request,
        [FromServices] ICommandHandler<WithdrawCommand, Result<Unit>> handler,
        CancellationToken ct)
    {
        var command = new WithdrawCommand(id, request.Amount);
        var result = await handler.Handle(command, ct);

        return result.ToHttpResult(Ok);
    }

    private static async Task<IResult> HandleTransfer(
        [FromBody] TransferRequest request,
        [FromServices] ICommandHandler<TransferCommand, Result<Unit>> handler,
        CancellationToken ct)
    {
        var command = new TransferCommand(request.FromAccountId, request.ToAccountId, request.Amount);
        var result = await handler.Handle(command, ct);

        return result.ToHttpResult(_ => Ok(new { message = "Transfer complete" }));
    }
}
