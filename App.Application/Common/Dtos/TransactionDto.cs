using App.Domain;

namespace App.Application.Common.Dtos;

public sealed record TransactionDto(
    Guid Id,
    Guid AccountId,
    TransactionType Type,
    decimal Amount,
    DateTime OccurredAtUtc,
    string? Description,
    Guid? CounterpartyAccountId
);