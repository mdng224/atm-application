using App.Domain;

namespace App.Infrastructure.Persistence.Seed;

internal static class AccountSeedFactory
{
    public static IEnumerable<Account> All =>
    [
        new(
            id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            name: "Checking",
            startingBalance: 1000m),

        new(
            id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            name: "Savings",
            startingBalance: 5000m)
    ];
}