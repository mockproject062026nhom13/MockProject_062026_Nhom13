namespace CRUDAccountDemo.Data.Configurations;

using CRUDAccountDemo.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.FullName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(a => a.Email)
            .IsUnique();

        builder.Property(a => a.Balance)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(a => a.CreatedAt)
            .IsRequired();

        builder.Property(a => a.UpdatedAt)
            .IsRequired();
    }
}
