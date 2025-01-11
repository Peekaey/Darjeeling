using Darjeeling.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Darjeeling.Repositories.EntityTypeConfigurations;

public class FCGuildRoleConfiguration : IEntityTypeConfiguration<FCGuildRole>
{
    public void Configure(EntityTypeBuilder<FCGuildRole> builder)
    {
        builder.HasKey(fgr => fgr.Id);
        builder.Property(fgr => fgr.RoleId).IsRequired();
        builder.Property(fgr => fgr.RoleName).IsRequired();
        builder.Property(fgr => fgr.DiscordGuildUid).IsRequired();
        builder.Property(fgr => fgr.DateCreated).IsRequired();
        builder.Property(fgr => fgr.RoleType).IsRequired();
        builder.HasOne(fgr => fgr.FCGuildServer)
            .WithOne(fgs => fgs.FCAdminRole)
            .HasForeignKey<FCGuildRole>(fgr => fgr.FCGuildServerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}