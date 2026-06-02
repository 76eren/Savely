using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class GameCollectionConfiguration : IEntityTypeConfiguration<GameCollection>
{
    public void Configure(EntityTypeBuilder<GameCollection> builder)
    {
        builder.HasKey(collection => collection.Id);
        builder.Property(collection => collection.Name)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(collection => collection.CreatedAt)
            .IsRequired();
        builder.Property(collection => collection.UpdatedAt)
            .IsRequired();
        builder.HasOne(collection => collection.User)
            .WithMany(user => user.Collections)
            .HasForeignKey(collection => collection.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
