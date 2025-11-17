using App.Application.Abstractions.Persistence.Readers;
using App.Application.Common.Dtos;
using App.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Persistence.Readers;

public sealed class TransactionReader(AppDbContext db) : ITransactionReader
{
    public async Task<(IReadOnlyList<TransactionDto> items, int totalCount)> GetPagedAsync(
        Guid id,
        int skip,
        int take,
        CancellationToken ct = default)
    {
        var query = db.ReadSet<Transaction>()
            .AsNoTracking()
            .Where(t => t.AccountId == id)
            .OrderByDescending(t => t.OccurredAtUtc);

        var items = await query
            .Skip(skip)
            .Take(take)
            .Select(t => new TransactionDto(
                t.Id,
                t.AccountId,
                t.Type,
                t.Amount,
                t.OccurredAtUtc,
                t.Description,
                t.CounterpartyAccountId
            ))
            .ToListAsync(ct);

        var totalCount = await query.CountAsync(ct);
        if (totalCount == 0 || skip >= totalCount)
            return ([], totalCount);
        
        return (items, totalCount);
    }
}