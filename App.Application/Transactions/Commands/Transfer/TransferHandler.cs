using App.Application.Abstractions.Handlers;
using App.Application.Abstractions.Persistence;
using App.Application.Abstractions.Persistence.Repositories;
using App.Application.Common.Results;
using static App.Application.Common.Results.R;

namespace App.Application.Transactions.Commands.Transfer;

public class TransferHandler(IAccountRepository repo, IUnitOfWork uow)
    : ICommandHandler<TransferCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(TransferCommand request, CancellationToken ct)
    {
        if (request.Amount <= 0)
            return Fail<Unit>("validation.amount", "Transfer amount must be greater than zero.");

        var fromAccount = await repo.GetByIdAsync(request.FromAccountId, ct);
        if (fromAccount is null)
            return Fail<Unit>("not_found.fromAccount", "Source account not found.");

        var toAccount = await repo.GetByIdAsync(request.ToAccountId, ct);
        if (toAccount is null)
            return Fail<Unit>("not_found.toAccount", "Destination account not found.");

        try
        {
            fromAccount.TransferTo(
                destination: toAccount,
                amount: request.Amount,
                timestampUtc: DateTime.UtcNow
            );
        }
        catch (InvalidOperationException ex)
        {
            return Fail<Unit>("conflict.insufficient_funds", ex.Message);
        }

        await uow.SaveChangesAsync(ct);

        return Ok(Unit.Value);
    }
}