using System;
using System.Collections.Generic;
using System.Text;

namespace TicketBooking.Domain.Entities.AddOns
{
    public class AddOnService
    {
        public string ServiceName { get; } = string.Empty;
        public decimal Price { get; }

        public AddOnService(string ServiceName, decimal Price)
        {
            if (string.IsNullOrWhiteSpace(ServiceName))
                throw new ArgumentException("Service name cannot be empty.", nameof(ServiceName));

            if (Price < 0)
                throw new ArgumentException("Price cannot be negative.", nameof(Price));

            this.ServiceName = ServiceName;
            this.Price = Price;
        }
    }
}
