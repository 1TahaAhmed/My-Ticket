using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketBooking.Domain.Entities.Ticketing;

namespace TicketBooking.Infrastructure.Configurations
{
    public class TicketConfig : BaseEntityConfiguration<TicketBooking.Domain.Entities.Ticketing.Ticket, Guid>
    {
        public override void Configure(EntityTypeBuilder<TicketBooking.Domain.Entities.Ticketing.Ticket> builder)
        {
            base.Configure(builder);

            builder.ToTable("Tickets");

            builder.Property(t => t.TicketCode)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(t => t.TicketCode)
                .IsUnique();

            builder.Property(t => t.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(t => t.TicketStatus)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.HasOne(t => t.Booking)
                .WithMany()
                .HasForeignKey(t => t.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.EventSeat)
                .WithMany()
                .HasForeignKey(t => t.EventSeatId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}