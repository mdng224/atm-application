using App.Application.Abstractions.Persistence.Repositories;
using App.Domain;

namespace App.Infrastructure.Persistence.Repositories;

public class TransactionRepository(AppDbContext db): ITransactionRepository
{
    public void Add(Transaction transaction) => db.Add(transaction);

    public void Update(Transaction transaction) => db.Update(transaction);
}