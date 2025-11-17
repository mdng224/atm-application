using App.Application.Common.Dtos;

namespace App.Application.Abstractions.Persistence.Readers;

public interface ITransactionReader
{
    Task<(IReadOnlyList<TransactionDto> items, int totalCount)>
        GetPagedAsync(Guid id, int skip, int take, CancellationToken ct = default);
}