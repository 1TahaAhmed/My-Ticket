using System;
using System.Collections.Generic;
using System.Linq;
using TicketBooking.Domain.BaseEntity;
using TicketBooking.Domain.Entities.AddOns;
using TicketBooking.Domain.Entities.Identity;
using TicketBooking.Domain.Entities.Promotions;
using TicketBooking.Domain.Enums;

namespace TicketBooking.Domain.Entities.Ticketing
{
    public class Booking : MBaseEntity
    {
        public Guid UserId { get; private set; }
        public ApplicationUser? User { get; private set; }
        public string BookingReference { get; private set; } = string.Empty;
        public BookingStatus Status { get; private set; } = BookingStatus.Reserved;

        public decimal SubTotal { get; private set; }
        public decimal DiscountAmount { get; private set; }
        public decimal TotalAmount { get; private set; }

        public DateTime ReservedAtUtc { get; private set; } = DateTime.UtcNow;
        public DateTime ExpiresAtUtc { get; private set; }

        // Encapsulated Collections
        private readonly List<BookingItem> _items = new();
        public IReadOnlyCollection<BookingItem> Items => _items.AsReadOnly();

        private readonly List<AddOnService> _addOnServices = new();
        public IReadOnlyCollection<AddOnService> AddOnServices => _addOnServices.AsReadOnly();

        public Guid? PromoCodeId { get; private set; }
        public PromoCode? PromoCode { get; private set; }

        private Booking() { }

        public Booking(Guid userId, int holdDurationMinutes = 10)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId is required.", nameof(userId));

            if (holdDurationMinutes <= 0)
                throw new ArgumentException("Hold duration must be greater than zero.", nameof(holdDurationMinutes));

            UserId = userId;
            PromoCodeId = null;
            DiscountAmount = 0;
            Status = BookingStatus.Reserved;
            ReservedAtUtc = DateTime.UtcNow;
            ExpiresAtUtc = ReservedAtUtc.AddMinutes(holdDurationMinutes);
            BookingReference = GenerateBookingReference();
        }

        public void AddItem(Guid eventSeatId, decimal price, string seatLabel, string sectionName)
        {
            EnsureCanModify();

            var item = new BookingItem(Id, eventSeatId, price, seatLabel, sectionName);
            _items.Add(item);

            RecalculateTotal();
        }

        public void AddAddOnService(AddOnService addOn)
        {
            EnsureCanModify();
            _addOnServices.Add(addOn);

            RecalculateTotal();
        }

        public void ApplyPromoCode(PromoCode promoCode)
        {
            EnsureCanModify();

            if (promoCode == null)
                throw new ArgumentNullException(nameof(promoCode));

            if (!promoCode.IsValid())
                throw new InvalidOperationException("Promo code is invalid or expired.");

            PromoCodeId = promoCode.Id;

            var rawDiscount = SubTotal * (promoCode.DiscountPercentage / 100m);
            DiscountAmount = Math.Min(rawDiscount, promoCode.MaxDiscountAmount);

            RecalculateTotal();
        }

        public void RemovePromoCode()
        {
            EnsureCanModify();
            PromoCodeId = null;
            DiscountAmount = 0;
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

        private void EnsureCanModify()
        {
            if (Status != BookingStatus.Reserved)
                throw new InvalidOperationException("Cannot modify a booking that is not in Reserved status.");

            if (IsExpired())
                throw new InvalidOperationException("Cannot modify an expired booking.");
        }

        private void RecalculateTotal()
        {
            var itemsTotal = _items.Sum(i => i.Price);
            var addOnsTotal = _addOnServices.Sum(a => a.Price);

            SubTotal = itemsTotal + addOnsTotal;
            TotalAmount = Math.Max(0, SubTotal - DiscountAmount);
        }

        private static string GenerateBookingReference()
        {
            return $"BK-{DateTime.UtcNow:yyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";
        }
    }
}