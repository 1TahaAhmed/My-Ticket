using System;
using TicketBooking.Domain.BaseEntity;
using TicketBooking.Domain.Entities.Identity;
using TicketBooking.Domain.Enums;

namespace TicketBooking.Domain.Entities.Ticketing
{
    public class TicketResaleListing : MBaseEntity
    {
        public Guid BookingItemId { get; private set; }
        public BookingItem? BookingItem { get; private set; }

        public Guid SellerId { get; private set; }
        public ApplicationUser? Seller { get; private set; }

        public decimal OriginalPrice { get; private set; }
        public decimal ResalePrice { get; private set; }

        public ResaleListingStatus Status { get; private set; } = ResaleListingStatus.Active;

        public DateTime ListedAtUtc { get; private set; } = DateTime.UtcNow;
        public DateTime? SoldAtUtc { get; private set; }
        public DateTime? CancelledAtUtc { get; private set; }

        private TicketResaleListing() { }

        public TicketResaleListing(
            Guid bookingItemId,
            Guid sellerId,
            decimal originalPrice,
            decimal resalePrice,
            decimal maxAllowedMultiplier = 1.5m)
        {
            if (bookingItemId == Guid.Empty)
                throw new ArgumentException("BookingItemId is required.", nameof(bookingItemId));

            if (sellerId == Guid.Empty)
                throw new ArgumentException("SellerId is required.", nameof(sellerId));

            if (originalPrice <= 0)
                throw new ArgumentException("Original price must be greater than zero.", nameof(originalPrice));

            if (resalePrice <= 0)
                throw new ArgumentException("Resale price must be greater than zero.", nameof(resalePrice));

            var maxAllowedPrice = originalPrice * maxAllowedMultiplier;
            if (resalePrice > maxAllowedPrice)
                throw new InvalidOperationException($"Resale price cannot exceed the maximum allowed limit of {maxAllowedPrice:C}.");

            BookingItemId = bookingItemId;
            SellerId = sellerId;
            OriginalPrice = originalPrice;
            ResalePrice = resalePrice;
            Status = ResaleListingStatus.Active;
            ListedAtUtc = DateTime.UtcNow;
        }

        public void UpdatePrice(decimal newPrice, decimal maxAllowedMultiplier = 1.5m)
        {
            if (Status != ResaleListingStatus.Active)
                throw new InvalidOperationException($"Cannot update price for a listing with status '{Status}'.");

            if (newPrice <= 0)
                throw new ArgumentException("Resale price must be greater than zero.", nameof(newPrice));

            var maxAllowedPrice = OriginalPrice * maxAllowedMultiplier;
            if (newPrice > maxAllowedPrice)
                throw new InvalidOperationException($"Resale price cannot exceed the maximum allowed limit of {maxAllowedPrice:C}.");

            ResalePrice = newPrice;
        }

        public void MarkAsSold()
        {
            if (Status != ResaleListingStatus.Active)
                throw new InvalidOperationException($"Cannot mark listing as sold from status '{Status}'.");

            Status = ResaleListingStatus.Sold;
            SoldAtUtc = DateTime.UtcNow;
        }

        public void CancelListing()
        {
            if (Status != ResaleListingStatus.Active)
                throw new InvalidOperationException($"Cannot cancel a listing with status '{Status}'.");

            Status = ResaleListingStatus.Cancelled;
            CancelledAtUtc = DateTime.UtcNow;
        }
    }
}