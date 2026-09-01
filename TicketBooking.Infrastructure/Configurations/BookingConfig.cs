using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TicketBooking.Domain.Entities.Ticketing;

namespace TicketBooking.Infrastructure.Configurations
{
    public class BookingConfig : BaseEntityConfiguration<Booking, Guid>
    {
        public override void Configure(EntityTypeBuilder<Booking> builder)
        {
            base.Configure(builder);

            builder.ToTable("Bookings");

            builder.HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(b => b.BookingReference)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(b => b.BookingReference)
                .IsUnique();

            builder.Property(b => b.SubTotal)
                .HasColumnType("decimal(18,2)");

            builder.Property(b => b.DiscountAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(b => b.TotalAmount)
                .HasColumnType("decimal(18,2)");

            builder.HasMany(b => b.Items)
                .WithOne(b => b.Booking)
                .HasForeignKey(b => b.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.PromoCode)
                .WithMany()
                .HasForeignKey(b => b.PromoCodeId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Metadata.FindNavigation(nameof(Booking.Items))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Metadata.FindNavigation(nameof(Booking.AddOnServices))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
