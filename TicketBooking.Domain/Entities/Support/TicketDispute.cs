using System;
using TicketBooking.Domain.BaseEntity;
using TicketBooking.Domain.Entities.Identity;
using TicketBooking.Domain.Entities.Ticketing;
using TicketBooking.Domain.Enums;

namespace TicketBooking.Domain.Entities.Support
{
    public class TicketDispute : MBaseEntity
    {
        public Guid RaisedById { get; private set; }
        public ApplicationUser? RaisedBy { get; private set; }

        public Guid? BookingId { get; private set; }
        public Booking? Booking { get; private set; }

        public Guid? ResaleTransactionId { get; private set; }
        public ResaleTransaction? ResaleTransaction { get; private set; }

        public string DisputeReason { get; private set; } = string.Empty;
        public DisputeStatus Status { get; private set; } = DisputeStatus.Open;
        public DisputeResolution? Resolution { get; private set; }
        public string? AdminNotes { get; private set; }

        public DateTime OpenedAtUtc { get; private set; } = DateTime.UtcNow;
        public DateTime? ClosedAtUtc { get; private set; }

        private TicketDispute() { }

        public TicketDispute(Guid raisedById, string disputeReason, Guid? bookingId = null, Guid? resaleTransactionId = null)
        {
            if (raisedById == Guid.Empty)
                throw new ArgumentException("RaisedById is required.", nameof(raisedById));

            if (string.IsNullOrWhiteSpace(disputeReason))
                throw new ArgumentException("Dispute reason is required.", nameof(disputeReason));

            if (!bookingId.HasValue && !resaleTransactionId.HasValue)
                throw new ArgumentException("Dispute must be linked to either a Booking or a ResaleTransaction.");

            RaisedById = raisedById;
            DisputeReason = disputeReason.Trim();
            BookingId = bookingId;
            ResaleTransactionId = resaleTransactionId;
            Status = DisputeStatus.Open;
            OpenedAtUtc = DateTime.UtcNow;
        }

        public void Resolve(DisputeResolution resolution, string adminNotes)
        {
            if (Status != DisputeStatus.Open && Status != DisputeStatus.UnderReview)
                throw new InvalidOperationException($"Cannot resolve dispute in '{Status}' status.");

            if (string.IsNullOrWhiteSpace(adminNotes))
                throw new ArgumentException("Admin notes are required for resolution.", nameof(adminNotes));

            Resolution = resolution;
            AdminNotes = adminNotes.Trim();
            Status = DisputeStatus.Resolved;
            ClosedAtUtc = DateTime.UtcNow;
        }

        public void MarkUnderReview()
        {
            if (Status != DisputeStatus.Open)
                throw new InvalidOperationException($"Cannot put dispute under review from '{Status}' status.");

            Status = DisputeStatus.UnderReview;
        }

        public void Reject(string adminNotes)
        {
            if (Status != DisputeStatus.Open && Status != DisputeStatus.UnderReview)
                throw new InvalidOperationException($"Cannot reject dispute in '{Status}' status.");

            if (string.IsNullOrWhiteSpace(adminNotes))
                throw new ArgumentException("Admin notes are required when rejecting a dispute.", nameof(adminNotes));

            Resolution = DisputeResolution.Rejected;
            AdminNotes = adminNotes.Trim();
            Status = DisputeStatus.Rejected;
            ClosedAtUtc = DateTime.UtcNow;
        }
    }
}