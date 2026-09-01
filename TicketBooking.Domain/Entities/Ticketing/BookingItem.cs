using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ticket;
using TicketBooking.Domain.BaseEntity;
using TicketBooking.Domain.Entities.Pricing;

namespace TicketBooking.Domain.Entities.Ticketing
{
    public class BookingItem : BaseEntity<Guid>
    {
        [Required, ForeignKey(nameof(Booking))]
        public Guid BookingId { get; private set; }
        public Booking? Booking { get; private set; }

        [Required, ForeignKey(nameof(EventSeat))]
        public Guid EventSeatId { get; private set; }
        public EventSeat? EventSeat { get; private set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; private set; }

        [Required, MaxLength(100)]
        public string SeatLabel { get; private set; } = string.Empty;

        [Required, MaxLength(100)]
        public string SectionName { get; private set; } = string.Empty;

        private BookingItem() { }

        public BookingItem(
            Guid bookingId,
            Guid eventSeatId,
            decimal price,
            string seatLabel,
            string sectionName)
        {
            if (bookingId == Guid.Empty)
                throw new ArgumentException("BookingId is required.", nameof(BookingId));

            if (eventSeatId == Guid.Empty)
                throw new ArgumentException("EventSeatId is required.", nameof(eventSeatId));

            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero.", nameof(price));

            if (string.IsNullOrWhiteSpace(seatLabel))
                throw new ArgumentException("Seat label cannot be empty.", nameof(seatLabel));

            if (string.IsNullOrWhiteSpace(sectionName))
                throw new ArgumentException("Section name cannot be empty.", nameof(sectionName));
            
            BookingId = bookingId;
            EventSeatId = eventSeatId;
            Price = price;
            SeatLabel = seatLabel.Trim();
            SectionName = sectionName.Trim();
        }
    }
}