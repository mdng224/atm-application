using App.Application.Abstractions.Handlers;
using App.Application.Abstractions.Persistence.Readers;
using App.Application.Common.Dtos;
using App.Application.Common.Results;
using static App.Application.Common.Results.R;

namespace App.Application.Accounts.Queries.GetAccounts;

public sealed class GetAccountsHandler(IAccountReader reader)
    : IQueryHandler<GetAccountsQuery, Result<List<AccountDto>>>
{
    public async Task< Result<List<AccountDto>>> Handle(GetAccountsQuery query, CancellationToken ct)
    {
        var accounts = await reader.GetAllAsync(query, ct);
        
        return Ok(accounts);
    }
}