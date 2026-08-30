using System;
using TicketBooking.Domain.BaseEntity;

namespace TicketBooking.Domain.Entities.Identity
{
    public class MembershipPlan : MBaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public int DurationInMonths { get; private set; }
        public decimal DiscountPercentage { get; private set; }
        public bool IsActive { get; private set; }

        private MembershipPlan() { }

        public MembershipPlan(string name, string description, decimal price, int durationInMonths, decimal discountPercentage)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Plan name is required.", nameof(name));

            if (price < 0)
                throw new ArgumentException("Price cannot be negative.", nameof(price));

            if (durationInMonths <= 0)
                throw new ArgumentException("Duration must be at least 1 month.", nameof(durationInMonths));

            if (discountPercentage < 0 || discountPercentage > 100)
                throw new ArgumentException("Discount percentage must be between 0 and 100.", nameof(discountPercentage));

            Name = name.Trim();
            Description = description?.Trim() ?? string.Empty;
            Price = price;
            DurationInMonths = durationInMonths;
            DiscountPercentage = discountPercentage;
            IsActive = true;
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}