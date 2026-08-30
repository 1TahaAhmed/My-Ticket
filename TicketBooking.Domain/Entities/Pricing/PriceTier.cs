using System;
using System.Collections.Generic;
using System.Text;
using Ticket;
using TicketBooking.Domain.BaseEntity;
using TicketBooking.Domain.Entities.Catalog;

namespace TicketBooking.Domain.Entities.Pricing
{
    public class PriceTier : BaseEntity<int>
    {
        public Guid EventScheduleId { get; private set; }
        public EventSchedule? EventSchedule { get; private set; }

        public string Name { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public string Description { get; private set; } = string.Empty;

        private PriceTier() { }

        public PriceTier(Guid eventScheduleId, string description, string name, decimal price)
        {
            if(eventScheduleId == Guid.Empty)
                throw new ArgumentException("EventScheduleId is required.", nameof(eventScheduleId));

            EventScheduleId = eventScheduleId;
            UpdateDetails(name, price, description);
        }

        public void UpdateDetails(string name, decimal price, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tier name cannot be empty.", nameof(name));

            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero.", nameof(price));

            Name = name.Trim();
            Price = price;
            Description = description?.Trim() ?? string.Empty;
        }
    }
}
