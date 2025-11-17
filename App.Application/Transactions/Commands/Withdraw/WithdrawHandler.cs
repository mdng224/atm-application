using App.Application.Abstractions.Handlers;
using App.Application.Abstractions.Persistence;
using App.Application.Abstractions.Persistence.Repositories;
using App.Application.Common.Results;
using static App.Application.Common.Results.R;

namespace App.Application.Transactions.Commands.Withdraw;

public class WithdrawHandler(
    IAccountRepository accounts,
    IUnitOfWork uow
) : ICommandHandler<WithdrawCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(WithdrawCommand request, CancellationToken ct)
    {
        if (request.Amount <= 0)
            return Fail<Unit>("validation.amount", "Withdrawal amount must be greater than zero.");

        var account = await accounts.GetByIdAsync(request.AccountId, ct);
        if (account is null)
            return Fail<Unit>("not_found.account", "Account not found.");

        try
        {
            account.Withdraw(amount: request.Amount, timestampUtc: DateTime.UtcNow);
        }
        catch (InvalidOperationException ex)
        {
            // Domain throws on insufficient funds
            return Fail<Unit>("conflict.insufficient_funds", ex.Message);
        }

        await uow.SaveChangesAsync(ct);

        return Ok(Unit.Value);
    }
}