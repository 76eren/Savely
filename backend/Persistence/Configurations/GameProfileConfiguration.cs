using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class GameProfileConfiguration : IEntityTypeConfiguration<GameProfile>
{
    public void Configure(EntityTypeBuilder<GameProfile> builder)
    {
        builder.HasKey(profile => profile.Id);
        builder.Property(profile => profile.Name)
            .HasMaxLength(225)
            .IsRequired();
        builder.Property(profile => profile.CreatedAt)
            .IsRequired();
        builder.Property(profile => profile.UpdatedAt)
            .IsRequired();
        builder.HasOne(profile => profile.GameCollection)
            .WithMany(collection => collection.Profiles)
            .HasForeignKey(profile => profile.GameCollectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
