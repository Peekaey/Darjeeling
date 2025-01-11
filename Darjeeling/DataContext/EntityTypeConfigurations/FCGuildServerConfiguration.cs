using Darjeeling.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Darjeeling.Repositories.EntityTypeConfigurations;

public class FCGuildServerConfiguration : IEntityTypeConfiguration<FCGuildServer>
{
    public void Configure(EntityTypeBuilder<FCGuildServer> builder)
    {
        builder.HasKey(fgs => fgs.Id);
        builder.Property(fgs => fgs.AdminRoleId).IsRequired();
        builder.Property(fgs => fgs.DiscordGuildUid).IsRequired();
        builder.Property(fgs => fgs.DateCreated).IsRequired();
        builder.HasMany(fg => fg.FCMembers)
            .WithOne(fm => fm.FCGuildServer)
            .HasForeignKey(fm => fm.FCGuildServerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}