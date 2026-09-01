using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TicketBooking.Domain.Entities.AddOns
{
    public class AddOnService
    {
        [Key]
        public Guid AddOnServiceId { get; private set; }
        public string ServiceName { get; } = string.Empty;
        public decimal Price { get; }
        
        private AddOnService() { }

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
