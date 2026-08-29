using System;
using TicketBooking.Domain.BaseEntity;
using TicketBooking.Domain.Entities.Pricing;
using TicketBooking.Domain.Enums;

namespace TicketBooking.Domain.Entities.Ticketing
{
    public class Ticket : MBaseEntity
    {
        public Guid OrderId { get; private set; }
        public Order? Order { get; private set; }

        public Guid EventSeatId { get; private set; }
        public EventSeat? EventSeat { get; private set; }

        public string TicketCode { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public TicketStatus TicketStatus { get; private set; } = TicketStatus.Issued;

        public DateTime IssuedAtUtc { get; private set; }
        public DateTime? UsedAtUtc { get; private set; }

        private Ticket() { }

        public Ticket(Guid orderId, Guid eventSeatId, decimal price)
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("OrderId is required.", nameof(orderId));

            if (eventSeatId == Guid.Empty)
                throw new ArgumentException("EventSeatId is required.", nameof(eventSeatId));

            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero.", nameof(price));

            OrderId = orderId;
            EventSeatId = eventSeatId;
            Price = price;

            TicketStatus = TicketStatus.Issued;
            IssuedAtUtc = DateTime.UtcNow;
            TicketCode = GenerateTicketCode();
        }

        public void UseTicket()
        {
            if (TicketStatus == TicketStatus.Used)
                throw new InvalidOperationException("Ticket has already been used.");

            if (TicketStatus == TicketStatus.Cancelled || TicketStatus == TicketStatus.Refunded)
                throw new InvalidOperationException($"Cannot use a ticket that is {TicketStatus}.");

            TicketStatus = TicketStatus.Used;
            UsedAtUtc = DateTime.UtcNow;
        }

        public void Cancel()
        {
            if (TicketStatus == TicketStatus.Used)
                throw new InvalidOperationException("Cannot cancel a ticket that has already been used at the venue.");

            if (TicketStatus == TicketStatus.Cancelled)
                throw new InvalidOperationException("Ticket is already cancelled.");

            TicketStatus = TicketStatus.Cancelled;
        }

        private static string GenerateTicketCode()
        {
            return $"TCK-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        }
    }
}