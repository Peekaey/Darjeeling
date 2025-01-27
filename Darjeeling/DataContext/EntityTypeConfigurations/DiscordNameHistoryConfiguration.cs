using Darjeeling.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Darjeeling.Repositories.EntityTypeConfigurations;

public class DiscordNameHistoryConfiguration
{
    public void Configure(EntityTypeBuilder<DiscordNameHistory> builder)
    {
        builder.HasKey(nh => nh.Id);
        builder.Property(nh => nh.DiscordUsername).IsRequired();
        builder.Property(e => e.DateAdded).IsRequired();
        builder.HasOne(nh => nh.FcGuildMember)
            .WithMany(fm => fm.DiscordNameHistories)
            .HasForeignKey(nh => nh.FCMemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}