using System;
using System.Collections.Generic;
using System.Text;
using TicketBooking.Domain.BaseEntity;

namespace TicketBooking.Domain.Entities.Pricing
{
    public class SeatLock : MBaseEntity
    {
        public Guid EventSeatId { get; private set; }
        public EventSeat? EventSeat { get; private set; }
        public Guid UserId { get; private set; }
        public string LockToken { get; private set; } = string.Empty;
        public DateTime LockedAtUtc { get; private set; }
        public DateTime ExpiresAtUtc { get; private set; }
        public bool IsExpired => DateTime.UtcNow > ExpiresAtUtc;

        private SeatLock() { }
        public SeatLock(Guid eventSeatId, Guid userId, int lockDurationMinutes = 10)
        {
            if (eventSeatId == Guid.Empty)
                throw new ArgumentException("EventSeatId is required.", nameof(eventSeatId));

            if (userId == Guid.Empty)
                throw new ArgumentException("UserId is required.", nameof(userId));

            if (lockDurationMinutes <= 0)
                throw new ArgumentException("Lock duration must be greater than zero.", nameof(lockDurationMinutes));

            EventSeatId = eventSeatId;
            UserId = userId;
            LockToken = Guid.NewGuid().ToString("N");

            LockedAtUtc = DateTime.UtcNow;
            ExpiresAtUtc = LockedAtUtc.AddMinutes(lockDurationMinutes);
        }

        public void ExtendLock(int additionalMinutes)
        {
            if (IsExpired)
                throw new InvalidOperationException("Cannot extend an expired lock.");

            if (additionalMinutes <= 0)
                throw new ArgumentException("Additional minutes must be greater than zero.", nameof(additionalMinutes));

            ExpiresAtUtc = ExpiresAtUtc.AddMinutes(additionalMinutes);
        }
    }
}
