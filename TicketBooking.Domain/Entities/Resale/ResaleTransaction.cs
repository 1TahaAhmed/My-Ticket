using System;
using TicketBooking.Domain.BaseEntity;
using TicketBooking.Domain.Entities.Identity;
using TicketBooking.Domain.Enums;

namespace TicketBooking.Domain.Entities.Ticketing
{
    public class ResaleTransaction : MBaseEntity
    {
        public Guid ResaleListingId { get; private set; }
        public TicketResaleListing? ResaleListing { get; private set; }

        public Guid BuyerId { get; private set; }
        public ApplicationUser? Buyer { get; private set; }

        public Guid SellerId { get; private set; }
        public ApplicationUser? Seller { get; private set; }

        public decimal SaleAmount { get; private set; }
        public decimal PlatformFee { get; private set; }         // عمولة المنصة
        public decimal SellerPayoutAmount { get; private set; } // صافي المستحق للبائع

        public TransactionStatus Status { get; private set; } = TransactionStatus.Pending;
        public ResalePayoutStatus PayoutStatus { get; private set; } = ResalePayoutStatus.Pending;

        public string? GatewayTransactionReference { get; private set; }
        public string? PayoutTransactionReference { get; private set; }

        public DateTime TransactedAtUtc { get; private set; } = DateTime.UtcNow;
        public DateTime? PaidOutAtUtc { get; private set; }

        private ResaleTransaction() { }

        public ResaleTransaction(
            Guid resaleListingId,
            Guid buyerId,
            Guid sellerId,
            decimal saleAmount,
            decimal platformFeePercentage = 10m)
        {
            if (resaleListingId == Guid.Empty)
                throw new ArgumentException("ResaleListingId is required.", nameof(resaleListingId));

            if (buyerId == Guid.Empty)
                throw new ArgumentException("BuyerId is required.", nameof(buyerId));

            if (sellerId == Guid.Empty)
                throw new ArgumentException("SellerId is required.", nameof(sellerId));

            if (buyerId == sellerId)
                throw new InvalidOperationException("Seller cannot purchase their own listed ticket.");

            if (saleAmount <= 0)
                throw new ArgumentException("Sale amount must be greater than zero.", nameof(saleAmount));

            ResaleListingId = resaleListingId;
            BuyerId = buyerId;
            SellerId = sellerId;
            SaleAmount = saleAmount;

            // حساب النسبة وصافي البائع
            PlatformFee = Math.Round(saleAmount * (platformFeePercentage / 100m), 2);
            SellerPayoutAmount = saleAmount - PlatformFee;

            Status = TransactionStatus.Pending;
            PayoutStatus = ResalePayoutStatus.Pending;
            TransactedAtUtc = DateTime.UtcNow;
        }

        public void CompleteTransaction(string gatewayTransactionReference)
        {
            if (Status != TransactionStatus.Pending)
                throw new InvalidOperationException($"Cannot complete transaction from status '{Status}'.");

            if (string.IsNullOrWhiteSpace(gatewayTransactionReference))
                throw new ArgumentException("Gateway transaction reference is required.", nameof(gatewayTransactionReference));

            Status = TransactionStatus.Success;
            GatewayTransactionReference = gatewayTransactionReference.Trim();
        }

        public void MarkAsFailed()
        {
            if (Status != TransactionStatus.Pending)
                throw new InvalidOperationException($"Cannot fail transaction from status '{Status}'.");

            Status = TransactionStatus.Failed;
        }

        public void MarkPayoutAsCompleted(string payoutReference)
        {
            if (Status != TransactionStatus.Success)
                throw new InvalidOperationException("Cannot payout seller for an uncompleted transaction.");

            if (PayoutStatus != ResalePayoutStatus.Pending)
                throw new InvalidOperationException($"Payout is already in '{PayoutStatus}' status.");

            if (string.IsNullOrWhiteSpace(payoutReference))
                throw new ArgumentException("Payout reference is required.", nameof(payoutReference));

            PayoutStatus = ResalePayoutStatus.Completed;
            PayoutTransactionReference = payoutReference.Trim();
            PaidOutAtUtc = DateTime.UtcNow;
        }
    }
}