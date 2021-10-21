using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reactivities.Domain.Entities;

namespace Reactivities.Infrastructure.Data.Config
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasOne(x => x.Activity)
                .WithMany(x => x.Comments)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
