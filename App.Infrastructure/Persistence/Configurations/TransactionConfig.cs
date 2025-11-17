using App.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infrastructure.Persistence.Configurations;

public sealed class TransactionConfig : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> b)
    {
        b.ToTable("transactions");
        b.HasKey(t => t.Id);
        b.Property(t => t.Id).ValueGeneratedNever();
        b.Property(t => t.Amount).HasColumnType("numeric(18,2)").IsRequired();
        b.Property(t => t.Description).HasMaxLength(500);
        b.Property(t => t.CounterpartyAccountId)
            .HasColumnName("counterparty_account_id");
        b.Property(t => t.OccurredAtUtc)
            .HasColumnName("occurred_at_utc")
            .HasColumnType("timestamp with time zone")
            .IsRequired();
        b.Property(t => t.Type)
            .HasColumnName("type")
            .HasConversion<int>()           // maps enum <-> int
            .IsRequired();
        b.HasOne<Account>()
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
        b.HasOne<Account>()
            .WithMany()
            .HasForeignKey(t => t.CounterpartyAccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}