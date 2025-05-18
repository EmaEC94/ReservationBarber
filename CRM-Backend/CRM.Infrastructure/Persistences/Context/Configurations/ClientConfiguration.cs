using CRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.Infrastructure.Persistences.Context.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(e => e.Id);
          

            builder.Property(e => e.Address).IsUnicode(false);
            builder.Property(e => e.DocumentNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            builder.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            builder.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.HasOne(d => d.DocumentType).WithMany(dt => dt.Clients)
                .HasForeignKey(c => c.DocumentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull);        

            builder.HasMany(c => c.Notifications)
                .WithOne(n => n.Client)
                .HasForeignKey(n => n.ClientId);

            builder.HasMany(c => c.ActivePauses)
               .WithOne(a => a.Client)
               .HasForeignKey(a => a.ClientId)
               .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
