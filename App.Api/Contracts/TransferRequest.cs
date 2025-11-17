namespace App.Api.Contracts;

public sealed record TransferRequest(Guid FromAccountId, Guid ToAccountId, decimal Amount);