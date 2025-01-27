using Darjeeling.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Darjeeling.Repositories.EntityTypeConfigurations;

public class NameHistoryConfiguration : IEntityTypeConfiguration<NameHistory>
{
    public void Configure(EntityTypeBuilder<NameHistory> builder)
    {
        builder.HasKey(nh => nh.Id);
        builder.Property(nh => nh.DiscordUserUid).IsRequired();
        builder.Property(nh => nh.FirstName).IsRequired();
        builder.Property(nh => nh.LastName).IsRequired();
        builder.Property(nh => nh.DateAdded).IsRequired();
        builder.HasOne(nh => nh.FcGuildMember)
            .WithMany(fm => fm.NameHistories)
            .HasForeignKey(nh => nh.FCMemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
}