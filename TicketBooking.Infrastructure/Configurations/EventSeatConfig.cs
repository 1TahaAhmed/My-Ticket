using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TicketBooking.Domain.Entities.Pricing;

namespace TicketBooking.Infrastructure.Configurations
{
    public class EventSeatConfig : BaseEntityConfiguration<EventSeat, Guid>
    {
        public override void Configure(EntityTypeBuilder<EventSeat> builder)
        {
            builder.Property(e => e.Price)
                  .HasColumnType("decimal(18,2)");

            builder.HasOne(e => e.EventSchedule)
                  .WithMany()
                  .HasForeignKey(e => e.EventScheduleId)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Seat)
                  .WithMany()
                  .HasForeignKey(e => e.SeatId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
