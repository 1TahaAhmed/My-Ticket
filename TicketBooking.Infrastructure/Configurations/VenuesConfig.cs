using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TicketBooking.Domain.Entities.Venues;

namespace TicketBooking.Infrastructure.Configurations
{
    public class VenuesConfig : BaseEntityConfiguration<Venue, int>
    {
        public override void Configure(EntityTypeBuilder<Venue> builder)
        {
            base.Configure(builder);

            builder.ToTable("Venues");

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.Description)
                .HasMaxLength(500);

            builder.Property(b => b.Address)
                .HasMaxLength(250);

            builder.Property(b => b.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.Country)
                .HasMaxLength(100);

            builder.Property(b => b.LocationUrl)
                .HasMaxLength(500);

            builder.Property(b => b.Gates)
                .IsRequired();

            builder.HasMany(b => b.Zones)
                .WithOne(b => b.Venue)
                .HasForeignKey(b => b.VenueId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Metadata.FindNavigation(nameof(Venue.Zones))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
