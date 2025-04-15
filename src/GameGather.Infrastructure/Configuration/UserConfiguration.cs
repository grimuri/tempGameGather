using GameGather.Domain.Aggregates.Users;
using GameGather.Domain.Aggregates.Users.Enums;
using GameGather.Domain.Aggregates.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameGather.Infrastructure.Configuration;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Id
        builder
            .HasKey(r => r.Id);

        builder
            .Property(r => r.Id)
            .HasConversion(
                c => c.Value,
                value => UserId.Create(value))
            .ValueGeneratedOnAdd();

        // Firstname
        builder
            .Property(r => r.FirstName)
            .HasMaxLength(100);

        // Lastname
        builder
            .Property(r => r.LastName)
            .HasMaxLength(100);
        
        // Email
        builder
            .Property(r => r.Email)
            .HasMaxLength(255);

        builder
            .HasIndex(r => r.Email)
            .IsUnique();
        
        // Password
        builder
            .ComplexProperty(r => r.Password);
        
        // VerificationToken
        builder
            .ComplexProperty(r => r.VerificationToken);
        
        // ResetPasswordToken
        builder
            .OwnsOne(r => r.ResetPasswordToken);
        
        // Ban
        builder
            .OwnsOne(r => r.Ban);
        
        // Role
        builder
            .Property(r => r.Role)
            .HasConversion(
                c => c.ToString(),
                c => (Role)Enum.Parse(typeof(Role), c))
            .HasColumnName("Role");
    }
}