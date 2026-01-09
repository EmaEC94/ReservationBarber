using CRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.Infrastructure.Persistences.Context.Configurations
{

    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
             .IsRequired()
             .HasMaxLength(1000)
             .IsUnicode(false);

            builder.Property(e => e.Note)
                .HasMaxLength(1000)
                .IsUnicode(false);

            builder.Property(e => e.Payment)
                .HasMaxLength(1000)
                .IsUnicode(false);

        }
    }
}
