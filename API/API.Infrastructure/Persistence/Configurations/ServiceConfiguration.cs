using API.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations;

public sealed class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(
        EntityTypeBuilder<Service> b
    )
    {
        b.ToTable("services");

        b.HasKey(x => x.Id);
        b.Property(x => x.Id)
            .ValueGeneratedNever(); // Guid asignado en dominio

        b.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        // Value Object como owned type portable
        b.OwnsOne(x => x.PortRange, pr =>
        {
            pr.Property(p => p.From)
              .HasColumnName("port_from")
              .IsRequired();
            pr.Property(p => p.To)
              .HasColumnName("port_to")
              .IsRequired();
        });

        b.HasIndex(x => x.Name).IsUnique(); // regla de unicidad (reforzada por handler)
    }
}
