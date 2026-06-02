using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class GameSaveConfiguration : IEntityTypeConfiguration<GameSave>
{
    public void Configure(EntityTypeBuilder<GameSave> builder)
    {
        builder.HasKey(save => save.Id);

        builder.Property(save => save.Name)
            .HasMaxLength(225)
            .IsRequired();

        builder.Property(save => save.Size)
            .IsRequired();

        builder.Property(save => save.CreatedAt)
            .IsRequired();

        builder.Property(save => save.UpdatedAt)
            .IsRequired();

        builder.HasOne(save => save.GameProfile)
            .WithMany(profile => profile.GameSaves)
            .HasForeignKey(save => save.GameProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
