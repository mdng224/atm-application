namespace App.Application.Transactions.Commands.Deposit;

public sealed record DepositCommand(Guid AccountId, decimal Amount);