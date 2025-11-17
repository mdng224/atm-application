using App.Application.Abstractions.Handlers;
using App.Application.Abstractions.Persistence.Readers;
using App.Application.Common.Dtos;
using App.Application.Common.Pagination;
using App.Application.Common.Results;
using static App.Application.Common.Results.R;

namespace App.Application.Transactions.Queries.GetTransactions;

public sealed class GetTransactionsHandler(ITransactionReader reader)
    : IQueryHandler<GetTransactionsQuery, Result<PagedResult<TransactionDto>>>
{
    public async Task<Result<PagedResult<TransactionDto>>> Handle(GetTransactionsQuery query, CancellationToken ct)
    {
        var (page, pageSize, skip) = query.PagedQuery;
        var (items, total) = await reader.GetPagedAsync(
            query.AccountId,
            skip,
            pageSize,
            ct);

        var pagedResult = new PagedResult<TransactionDto>(items, total, page, pageSize);
        
        return Ok(pagedResult);
    }
}