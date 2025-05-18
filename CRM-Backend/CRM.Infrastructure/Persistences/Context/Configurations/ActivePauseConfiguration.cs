using CRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.Infrastructure.Persistences.Context.Configurations
{
    public class ActivePauseConfiguration : IEntityTypeConfiguration<ActivePause>
    {
        public void Configure(EntityTypeBuilder<ActivePause> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(ap => ap.Notifications)
                .WithOne(n => n.ActivePause)
                .HasForeignKey(n => n.ActivePauseId);
        }
    }
}
