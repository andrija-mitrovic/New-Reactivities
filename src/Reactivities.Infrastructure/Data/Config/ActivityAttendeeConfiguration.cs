using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reactivities.Domain.Entities;

namespace Reactivities.Infrastructure.Data.Config
{
    public class ActivityAttendeeConfiguration : IEntityTypeConfiguration<ActivityAttendee>
    {
        public void Configure(EntityTypeBuilder<ActivityAttendee> builder)
        {
            builder.HasKey(x => new { x.AppUserId, x.ActivityId });

            builder.HasOne(x => x.AppUser)
                   .WithMany(x => x.ActivityAttendees)
                   .HasForeignKey(x => x.AppUserId);

            builder.HasOne(x => x.Activity)
                   .WithMany(x => x.ActivityAttendees)
                   .HasForeignKey(x => x.ActivityId);
        }
    }
}
