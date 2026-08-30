using System;
using System.Collections.Generic;
using System.Text;
using Ticket;
using TicketBooking.Domain.Entities.Catalog;
using TicketBooking.Domain.Entities.Venues;
using TicketBooking.Domain;
using TicketBooking.Domain.BaseEntity;

namespace TicketBooking.Domain.Entities.Pricing
{
    public class EventSeat : MBaseEntity
    {
        public Guid EventScheduleId { get; private set; }
        public EventSchedule? EventSchedule { get; private set; }
        public Guid SeatId { get; private set; }
        public Seat? Seat { get; private set; }
        public decimal Price { get; private set; }
        public EventSeatStatus SeatStatus { get; private set; } = EventSeatStatus.Available;

        private EventSeat() { }

        public EventSeat(decimal price,
            Guid seatId,
            Guid eventScheduleId)
        {
            Price = price;
            SeatId = seatId;
            EventScheduleId = eventScheduleId;

            if (seatId == Guid.Empty)
                throw new ArgumentException("SeatId is required.", nameof(seatId));

            if (eventScheduleId == Guid.Empty)
                throw new ArgumentException("EventScheduleId is required.", nameof(eventScheduleId));

            UpdateDetails(price);
        }

        public void UpdateDetails(decimal newPrice)
        {
            if (SeatStatus == EventSeatStatus.Sold)
                throw new InvalidOperationException("Cannot update price for a seat that is already sold.");

            if (newPrice <= 0)
                throw new ArgumentException("The price must be greater than zero.", nameof(newPrice));

            Price = newPrice;
        }

        public void Reserve()
        {
            if (SeatStatus != EventSeatStatus.Available)
                throw new InvalidOperationException($"Cannot reserve seat. Current status is {SeatStatus}.");

            SeatStatus = EventSeatStatus.Reserved;
        }

        public void ConfirmBookinging()
        {
            if (SeatStatus != EventSeatStatus.Reserved)
                throw new InvalidOperationException($"Cannot confirm Bookinging. Seat must be in Reserved status, but was {SeatStatus}.");

            SeatStatus = EventSeatStatus.Sold;
        }

        public void Release()
        {
            if (SeatStatus != EventSeatStatus.Reserved)
                throw new InvalidOperationException($"Cannot release seat. Only Reserved seats can be released, current status is {SeatStatus}.");

            SeatStatus = EventSeatStatus.Available;
        }

        public void Block()
        {
            if (SeatStatus == EventSeatStatus.Sold)
                throw new InvalidOperationException("Cannot block a seat that is already sold.");

            SeatStatus = EventSeatStatus.Blocked;
        }

        public void Unblock()
        {
            if (SeatStatus != EventSeatStatus.Blocked)
                throw new InvalidOperationException("Seat is not currently blocked.");

            SeatStatus = EventSeatStatus.Available;
        }
    }
}
