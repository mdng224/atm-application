using App.Application.Abstractions.Persistence.Repositories;
using App.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Persistence.Repositories;

public class AccountRepository(AppDbContext db) : IAccountRepository
{
    public Task<Account?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        db.Accounts.AsTracking().SingleOrDefaultAsync(a => a.Id == id, ct);
}