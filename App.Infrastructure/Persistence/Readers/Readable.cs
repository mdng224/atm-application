using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Persistence.Readers;

internal static class Readable
{
    public static IQueryable<T> ReadSet<T>(this DbContext db)
        where T : class
    {
        return db.Set<T>().AsNoTracking();
    }
}