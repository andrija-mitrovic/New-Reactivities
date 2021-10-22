using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reactivities.Domain.Entities;

namespace Reactivities.Infrastructure.Data.Config
{
    public class UserFollowingConfiguration : IEntityTypeConfiguration<UserFollowing>
    {
        public void Configure(EntityTypeBuilder<UserFollowing> builder)
        {
            builder.HasKey(x => new { x.ObserverId, x.TargetId });

            builder.HasOne(x => x.Observer)
                .WithMany(x => x.Followings)
                .HasForeignKey(x => x.ObserverId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Target)
                .WithMany(x => x.Followers)
                .HasForeignKey(x => x.TargetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
