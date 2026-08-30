using System;
using System.Collections.Generic;
using System.Text;
using TicketBooking.Domain.BaseEntity;
using TicketBooking.Domain.Entities.Ticketing;
using TicketBooking.Domain.Enums;

namespace TicketBooking.Domain.Entities.Payments
{
    public class PaymentTransaction : MBaseEntity
    {
        public Guid BookingId { get; private set; }
        public Booking? Booking { get; private set; }
        public string TransactionRef { get; set; } = string.Empty;
        public PaymentMethod PaymentMethod { get; private set; } = PaymentMethod.Cash;
        public decimal Amount { get; private set; }
        public TransactionStatus TransactionStatus { get; private set; }
        public string FailureReason { get; private set; } = default!;

        private PaymentTransaction() { }
        public PaymentTransaction(Guid bookingId,PaymentMethod paymentMethod, string transactionRef, decimal amount, string failureReason)
        {
            if (bookingId == Guid.Empty)
                throw new ArgumentException("BookingId is required.", nameof(bookingId));

            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

            BookingId = bookingId;
            Amount = amount;
            PaymentMethod = paymentMethod;
            TransactionStatus = TransactionStatus.Pending;
        }

        public void MarkAsSuccess(string transactionRef)
        {
            if (string.IsNullOrWhiteSpace(transactionRef))
                throw new ArgumentException("Transaction reference is required.", nameof(transactionRef));

            if (TransactionStatus != TransactionStatus.Pending)
                throw new InvalidOperationException($"Cannot mark transaction as successfrom status {TransactionStatus}.");

            TransactionRef = transactionRef.Trim();
            TransactionStatus = TransactionStatus.Success;
        }

        public void MarkAsFailed(string reason)
        {
            if (TransactionStatus != TransactionStatus.Pending)
                throw new InvalidOperationException($"Cannot mark transaction as failed from status {TransactionStatus}.");

            FailureReason = string.IsNullOrWhiteSpace(reason) ? "Unknown error" : reason.Trim();
            TransactionStatus = TransactionStatus.Failed;
        }
    }
}
