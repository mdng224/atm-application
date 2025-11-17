namespace App.Domain;

public sealed class Transaction
{
    public Guid Id { get; }
    public Guid AccountId { get; }
    public TransactionType Type { get; }
    public decimal Amount { get; }
    public DateTime OccurredAtUtc { get; }
    public string? Description { get; }
    public Guid? CounterpartyAccountId { get; }
    
    private Transaction() { } // EF
    public Transaction(
        Guid accountId,
        TransactionType type,
        decimal amount,
        DateTime occurredAtUtc,
        string? description,
        Guid? counterpartyAccountId)
    {
        if (accountId == Guid.Empty)
            throw new ArgumentException("Account id cannot be empty.", nameof(accountId));

        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");

        Id = Guid.CreateVersion7();
        AccountId = accountId;
        Type = type;
        Amount = amount;
        OccurredAtUtc = occurredAtUtc;
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        CounterpartyAccountId = counterpartyAccountId;
    }
}