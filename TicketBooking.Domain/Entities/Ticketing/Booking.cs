using System;
using System.Collections.Generic;
using System.Linq;
using TicketBooking.Domain.BaseEntity;
using TicketBooking.Domain.Enums;

namespace TicketBooking.Domain.Entities.Ticketing
{
    public class Booking : BaseEntity<Guid>
    {
        public Guid UserId { get; private set; }
        public string BookingReference { get; private set; } = string.Empty;
        public BookingStatus Status { get; private set; } = BookingStatus.Reserved;

        public decimal TotalAmount { get; private set; }
        public DateTime ReservedAtUtc { get; private set; } = DateTime.UtcNow;
        public DateTime ExpiresAtUtc { get; private set; }

        // Encapsulated Items Collection
        private readonly List<BookingItem> _items = new();
        public IReadOnlyCollection<BookingItem> Items => _items.AsReadOnly();

        private Booking() { }

        public Booking(Guid userId, int holdDurationMinutes = 10)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId is required.", nameof(userId));

            if (holdDurationMinutes <= 0)
                throw new ArgumentException("Hold duration must be greater than zero.", nameof(holdDurationMinutes));

            UserId = userId;
            Status = BookingStatus.Reserved;
            ReservedAtUtc = DateTime.UtcNow;
            ExpiresAtUtc = ReservedAtUtc.AddMinutes(holdDurationMinutes);
            BookingReference = GenerateBookingReference();
        }

        public void AddItem(Guid eventSeatId, decimal price, string seatLabel, string sectionName)
        {
            if (Status != BookingStatus.Reserved)
                throw new InvalidOperationException("Cannot add items to a booking that is not in Reserved status.");

            if (IsExpired())
                throw new InvalidOperationException("Cannot add items to an expired booking.");

            var item = new BookingItem(Id, eventSeatId, price, seatLabel, sectionName);
            _items.Add(item);

            RecalculateTotal();
        }

        public bool IsExpired()
        {
            return Status == BookingStatus.Reserved && DateTime.UtcNow > ExpiresAtUtc;
        }

        public void ConfirmPayment()
        {
            if (IsExpired())
                throw new InvalidOperationException("Cannot confirm payment for an expired booking.");

            if (Status != BookingStatus.Reserved)
                throw new InvalidOperationException($"Cannot confirm booking from status '{Status}'.");

            if (!_items.Any())
                throw new InvalidOperationException("Cannot confirm a booking with no items.");

            Status = BookingStatus.Confirmed;
        }

        public void Cancel()
        {
            if (Status == BookingStatus.Confirmed)
                throw new InvalidOperationException("Confirmed bookings cannot be cancelled without refund domain logic.");

            Status = BookingStatus.Cancelled;
        }

        public void Expire()
        {
            if (Status == BookingStatus.Reserved)
            {
                Status = BookingStatus.Expired;
            }
        }

        private void RecalculateTotal()
        {
            TotalAmount = _items.Sum(i => i.Price);
        }

        private static string GenerateBookingReference()
        {
            return $"BK-{DateTime.UtcNow:yyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";
        }
    }
}