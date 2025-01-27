using Darjeeling.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Darjeeling.Repositories.EntityTypeConfigurations;

public class LodestoneNameHistoryConfiguration : IEntityTypeConfiguration<LodestoneNameHistory>
{
    public void Configure(EntityTypeBuilder<LodestoneNameHistory> builder)
    {
        builder.HasKey(nh => nh.Id);
        builder.Property(nh => nh.FirstName).IsRequired();
        builder.Property(nh => nh.LastName).IsRequired();
        builder.Property(nh => nh.DateAdded).IsRequired();
        builder.HasOne(nh => nh.FcGuildMember)
            .WithMany(fm => fm.LodestoneNameHistories)
            .HasForeignKey(nh => nh.FCMemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
}