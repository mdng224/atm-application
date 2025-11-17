using App.Domain;

namespace App.Application.Abstractions.Persistence.Repositories;

public interface ITransactionRepository
{
    void Add(Transaction transaction);
    
    void Update(Transaction transaction);
}