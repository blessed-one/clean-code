using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(user => user.Id);
        
        builder.Property(user => user.Login)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(user => user.PasswordHash)
            .IsRequired()
            .HasMaxLength(70);

        builder
            .HasMany(user => user.Documents)
            .WithOne(document => document.Author)
            .HasForeignKey(document => document.AuthorId);
        
        builder.HasIndex(x => x.Login)
            .IsUnique();
    }
}