using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Persistence.Seed;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db, CancellationToken ct = default)
    {
        await SeedIfEmptyAsync(db, AccountSeedFactory.All, ct);

        await db.SaveChangesAsync(ct); // AuditSaveChangesInterceptor will stamp
    }
    
    private static async Task SeedIfEmptyAsync<TEntity>(
        AppDbContext db,
        IEnumerable<TEntity> seedData,
        CancellationToken ct)
        where TEntity : class
    {
        var set = db.Set<TEntity>();

        if (await set.AnyAsync(ct))
        {
            Console.WriteLine($"Skipping {typeof(TEntity).Name} seeding — already has data.");
            return;
        }

        var list = seedData as IList<TEntity> ?? seedData.ToList();
        set.AddRange(list);
        Console.WriteLine($"Seeded {typeof(TEntity).Name} records.");
    }
}