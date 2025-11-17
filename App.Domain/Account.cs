namespace App.Domain;

public sealed class Account
{
    public Guid Id { get; }
    public string Name { get; private set; } = null!;
    public decimal Balance { get; private set; }

    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();
    private readonly List<Transaction> _transactions = [];

    private Account() { } // EF
    public Account(Guid id, string name, decimal startingBalance = 0m)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Account id cannot be empty.", nameof(id));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Account name is required.", nameof(name));

        if (startingBalance < 0)
            throw new ArgumentOutOfRangeException(nameof(startingBalance), "Starting balance cannot be negative.");

        Id = id;
        SetName(name);
        Balance = startingBalance;

        if (startingBalance > 0)
        {
            AddTransaction(
                type: TransactionType.Deposit,
                amount: startingBalance,
                occurredAtUtc: DateTime.UtcNow,
                description: "Initial balance",
                counterpartyAccountId: null);
        }
    }

    public void Deposit(decimal amount, DateTime timestampUtc, string? description = null)
    {
        EnsurePositiveAmount(amount);

        Balance += amount;

        AddTransaction(
            type: TransactionType.Deposit,
            amount: amount,
            occurredAtUtc: timestampUtc,
            description: description ?? "Deposit",
            counterpartyAccountId: null);
    }

    public void Withdraw(decimal amount, DateTime timestampUtc, string? description = null)
    {
        EnsurePositiveAmount(amount);

        if (Balance < amount)
            throw new InvalidOperationException("Insufficient funds.");

        Balance -= amount;

        AddTransaction(
            type: TransactionType.Withdrawal,
            amount: amount,
            occurredAtUtc: timestampUtc,
            description: description ?? "Withdrawal",
            counterpartyAccountId: null);
    }

    public void TransferTo(Account destination, decimal amount, DateTime timestampUtc, string? description = null)
    {
        ArgumentNullException.ThrowIfNull(destination);

        if (ReferenceEquals(this, destination) || destination.Id == Id)
            throw new InvalidOperationException("Cannot transfer to the same account.");

        EnsurePositiveAmount(amount);

        if (Balance < amount)
            throw new InvalidOperationException("Insufficient funds for transfer.");

        // Withdraw from this account
        Balance -= amount;
        AddTransaction(
            type: TransactionType.TransferOut,
            amount: amount,
            occurredAtUtc: timestampUtc,
            description: description ?? $"Transfer to {destination.Name}",
            counterpartyAccountId: destination.Id);

        // Deposit into destination account
        destination.Balance += amount;
        destination.AddTransaction(
            type: TransactionType.TransferIn,
            amount: amount,
            occurredAtUtc: timestampUtc,
            description: description ?? $"Transfer from {Name}",
            counterpartyAccountId: Id);
    }

    private static void EnsurePositiveAmount(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
    }

    private void AddTransaction(
        TransactionType type,
        decimal amount,
        DateTime occurredAtUtc,
        string? description,
        Guid? counterpartyAccountId)
    {
        var transaction = new Transaction(
            accountId: Id,
            type: type,
            amount: amount,
            occurredAtUtc: occurredAtUtc,
            description: description,
            counterpartyAccountId: counterpartyAccountId);

        _transactions.Add(transaction);
    }
    
    private void SetName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Account name is required.", nameof(newName));

        Name = newName.Trim();
    }
}
