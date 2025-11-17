
using App.Application.Accounts.Queries.GetAccounts;
using App.Application.Common.Dtos;

namespace App.Application.Abstractions.Persistence.Readers;

public interface IAccountReader
{
    Task<List<AccountDto>> GetAllAsync(GetAccountsQuery query, CancellationToken ct = default);
}