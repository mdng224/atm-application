using App.Application.Abstractions.Handlers;
using App.Application.Abstractions.Persistence;
using App.Application.Abstractions.Persistence.Repositories;
using App.Application.Common.Results;
using static App.Application.Common.Results.R;

namespace App.Application.Transactions.Commands.Deposit;

public class DepositHandler(IAccountRepository accounts, IUnitOfWork uow)
    : ICommandHandler<DepositCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DepositCommand request, CancellationToken ct)
    {
        if (request.Amount <= 0)
            return Fail<Unit>("validation.amount", "Deposit amount must be greater than zero.");

        var account = await accounts.GetByIdAsync(request.AccountId, ct);
        if (account is null)
            return Fail<Unit>("not_found.account", "Account not found.");

        account.Deposit(
            amount: request.Amount,
            timestampUtc: DateTime.UtcNow
        );

        await uow.SaveChangesAsync(ct);

        return Ok(Unit.Value);
    }
}