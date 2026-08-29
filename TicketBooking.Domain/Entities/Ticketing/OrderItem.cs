using System;
using TicketBooking.Domain.BaseEntity;
using TicketBooking.Domain.Entities.Pricing;

namespace TicketBooking.Domain.Entities.Ticketing
{
    public class OrderItem : BaseEntity<int>
    {
        public Guid OrderId { get; private set; }
        public Order? Order { get; private set; }

        public Guid EventSeatId { get; private set; }
        public EventSeat? EventSeat { get; private set; }

        public decimal Price { get; private set; }
        public string SeatLabel { get; private set; } = string.Empty;
        public string SectionName { get; private set; } = string.Empty;

        private OrderItem() { }

        public OrderItem(
            Guid orderId,
            Guid eventSeatId,
            decimal price,
            string seatLabel,
            string sectionName)
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("OrderId is required.", nameof(orderId));

            if (eventSeatId == Guid.Empty)
                throw new ArgumentException("EventSeatId is required.", nameof(eventSeatId));

            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero.", nameof(price));

            if (string.IsNullOrWhiteSpace(seatLabel))
                throw new ArgumentException("Seat label cannot be empty.", nameof(seatLabel));

            if (string.IsNullOrWhiteSpace(sectionName))
                throw new ArgumentException("Section name cannot be empty.", nameof(sectionName));

            OrderId = orderId;
            EventSeatId = eventSeatId;
            Price = price;
            SeatLabel = seatLabel.Trim();
            SectionName = sectionName.Trim();
        }
    }
}