using System;
using TicketBooking.Domain.BaseEntity;

namespace TicketBooking.Domain.Entities.Promotions
{
    public class PromoCode : MBaseEntity
    {
        public string Code { get; private set; } = string.Empty;
        public decimal DiscountPercentage { get; private set; }
        public decimal MaxDiscountAmount { get; private set; }
        public int MaxUsageCount { get; private set; }
        public int CurrentUsageCount { get; private set; }
        public DateTime StartDateUtc { get; private set; }
        public DateTime EndDateUtc { get; private set; }
        public bool IsActive { get; private set; }

        private PromoCode() { }

        public PromoCode(string code, decimal discountPercentage, decimal maxDiscountAmount, int maxUsageCount, DateTime startDateUtc, DateTime endDateUtc)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Promo code is required.", nameof(code));

            if (discountPercentage <= 0 || discountPercentage > 100)
                throw new ArgumentException("Discount percentage must be between 1 and 100.", nameof(discountPercentage));

            Code = code.Trim().ToUpperInvariant();
            DiscountPercentage = discountPercentage;
            MaxDiscountAmount = maxDiscountAmount;
            MaxUsageCount = maxUsageCount;
            StartDateUtc = startDateUtc;
            EndDateUtc = endDateUtc;
            CurrentUsageCount = 0;
            IsActive = true;
        }

        public bool IsValid()
        {
            var now = DateTime.UtcNow;
            return IsActive && now >= StartDateUtc && now <= EndDateUtc && CurrentUsageCount < MaxUsageCount;
        }

        public void IncrementUsage()
        {
            if (!IsValid())
                throw new InvalidOperationException("Promo code is invalid or has reached its usage limit.");

            CurrentUsageCount++;
        }
    }
}