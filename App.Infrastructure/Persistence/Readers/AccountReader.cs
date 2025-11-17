using App.Application.Abstractions.Persistence.Readers;
using App.Application.Accounts.Queries.GetAccounts;
using App.Application.Common.Dtos;
using App.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Persistence.Readers;

public sealed class AccountReader(AppDbContext db) : IAccountReader
{
    public Task<List<AccountDto>> GetAllAsync(GetAccountsQuery query, CancellationToken ct = default)
    {
        var accounts = db.ReadSet<Account>()
            .Select(a => new AccountDto(
                a.Id,
                a.Name,
                a.Balance))
            .ToListAsync(ct);

        return accounts;
    }
}