namespace App.Application.Transactions.Commands.Transfer;

public sealed record TransferCommand(Guid FromAccountId, Guid ToAccountId, decimal Amount);