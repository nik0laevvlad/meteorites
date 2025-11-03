using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTask.Core.Models.Entities;

namespace TestTask.Infrastructure;

public class MeteoriteConfiguration : IEntityTypeConfiguration<Meteorite>
{
    public void Configure(EntityTypeBuilder<Meteorite> builder)
    {
        builder.ToTable("Meteorites");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Name).HasMaxLength(250);

        builder.OwnsOne(x => x.Geolocation, geo =>
        {
            geo.Property(g => g.Type)
                .HasColumnName("Geo_Type");

            geo.OwnsOne(g => g.Coordinates, coords =>
            {
                coords.Property(c => c.Lat)
                      .HasColumnName("Geo_Lat");

                coords.Property(c => c.Lon)
                      .HasColumnName("Geo_Lon");
            });
        });

        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => x.RecClass);
        builder.HasIndex(x => x.Year);

        builder.HasIndex(x => new { x.Year, x.RecClass, x.Name });
    }
}
