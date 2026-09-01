using System;
using TicketBooking.Domain.BaseEntity;
using TicketBooking.Domain.Entities.Payments;
using TicketBooking.Domain.Enums;

namespace TicketBooking.Domain.Entities.Ticketing
{
    public class RefundTransaction : MBaseEntity
    {
        public Guid PaymentTransactionId { get; private set; }
        public PaymentTransaction? PaymentTransaction { get; private set; }

        public Guid BookingId { get; private set; }
        public Booking? Booking { get; private set; }

        public decimal Amount { get; private set; }
        public string Reason { get; private set; } = string.Empty;
        public RefundStatus Status { get; private set; } = RefundStatus.Pending;
        public string? GatewayRefundReference { get; private set; }
        public DateTime RequestedAtUtc { get; private set; } = DateTime.UtcNow;
        public DateTime? ProcessedAtUtc { get; private set; }
        public string? FailureReason { get; private set; }

        private RefundTransaction() { }

        public RefundTransaction(Guid paymentTransactionId, Guid bookingId, decimal amount, string reason)
        {
            if (paymentTransactionId == Guid.Empty)
                throw new ArgumentException("PaymentTransactionId is required.", nameof(paymentTransactionId));

            if (bookingId == Guid.Empty)
                throw new ArgumentException("BookingId is required.", nameof(bookingId));

            if (amount <= 0)
                throw new ArgumentException("Refund amount must be greater than zero.", nameof(amount));

            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Refund reason is required.", nameof(reason));

            PaymentTransactionId = paymentTransactionId;
            BookingId = bookingId;
            Amount = amount;
            Reason = reason.Trim();
            Status = RefundStatus.Pending;
            RequestedAtUtc = DateTime.UtcNow;
        }

        public void MarkAsCompleted(string gatewayRefundReference)
        {
            if (Status != RefundStatus.Pending)
                throw new InvalidOperationException($"Cannot complete refund from status '{Status}'.");

            if (string.IsNullOrWhiteSpace(gatewayRefundReference))
                throw new ArgumentException("Gateway refund reference is required.", nameof(gatewayRefundReference));

            Status = RefundStatus.Completed;
            GatewayRefundReference = gatewayRefundReference.Trim();
            ProcessedAtUtc = DateTime.UtcNow;
        }

        public void MarkAsFailed(string failureReason)
        {
            if (Status != RefundStatus.Pending)
                throw new InvalidOperationException($"Cannot fail refund from status '{Status}'.");

            if (string.IsNullOrWhiteSpace(failureReason))
                throw new ArgumentException("Failure reason is required.", nameof(failureReason));

            Status = RefundStatus.Failed;
            FailureReason = failureReason.Trim();
            ProcessedAtUtc = DateTime.UtcNow;
        }
    }
}