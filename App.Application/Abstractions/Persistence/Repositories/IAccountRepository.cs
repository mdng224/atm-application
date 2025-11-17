
using App.Domain;

namespace App.Application.Abstractions.Persistence.Repositories;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid id, CancellationToken ct = default);
}