using Darjeeling.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Darjeeling.Repositories.EntityTypeConfigurations;

public class FCMemberConfiguration : IEntityTypeConfiguration<FCMember>
{
    public void Configure(EntityTypeBuilder<FCMember> builder)
    {
        builder.HasKey(fm => fm.Id);
        builder.Property(fm => fm.DiscordUserUId).IsRequired();
        builder.Property(fm => fm.DiscordUsername).IsRequired();
        builder.Property(fm => fm.LodestoneId).IsRequired(false);
        builder.Property(fm => fm.DateCreated).IsRequired();
        builder.HasMany(fm => fm.NameHistories)
            .WithOne(nh => nh.FCMember)
            .HasForeignKey(nh => nh.FCMemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}