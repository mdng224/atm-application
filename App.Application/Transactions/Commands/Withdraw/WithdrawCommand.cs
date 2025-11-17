namespace App.Application.Transactions.Commands.Withdraw;

public sealed record WithdrawCommand(Guid AccountId,  decimal Amount);