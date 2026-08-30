using System;
using TicketBooking.Domain.BaseEntity;

namespace TicketBooking.Domain.Entities.Identity
{
    public class BlacklistedAttendee : MBaseEntity
    {
        public string Email { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public string Reason { get; private set; } = string.Empty;
        public DateTime BlacklistedAtUtc { get; private set; }
        public bool IsActive { get; private set; }

        private BlacklistedAttendee() { }

        public BlacklistedAttendee(string email, string phoneNumber, string reason)
        {
            if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Either Email or PhoneNumber must be provided for blacklisting.");

            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Reason for blacklisting is required.", nameof(reason));

            Email = email?.Trim().ToLowerInvariant() ?? string.Empty;
            PhoneNumber = phoneNumber?.Trim() ?? string.Empty;
            Reason = reason.Trim();
            BlacklistedAtUtc = DateTime.UtcNow;
            IsActive = true;
        }

        public void RemoveFromBlacklist()
        {
            IsActive = false;
        }

        public void ReactivateBlacklist(string newReason)
        {
            if (string.IsNullOrWhiteSpace(newReason))
                throw new ArgumentException("New reason is required to reactivate blacklist.", nameof(newReason));

            Reason = newReason.Trim();
            IsActive = true;
        }
    }
}