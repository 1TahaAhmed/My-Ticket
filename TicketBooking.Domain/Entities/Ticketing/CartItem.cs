using System;
using System.Collections.Generic;
using System.Text;
using TicketBooking.Domain.BaseEntity;
using TicketBooking.Domain.Entities.Pricing;
using TicketBooking.Domain.Entities.Ticketing;
using TicketBookinging.Domain.Entities.Ticketing;

namespace TicketBooking.Domain.Entities.Ticketing
{
    public class CartItem : BaseEntity<int>
    {
        public Guid CartId { get; private set; }
        public Guid EventSeatId { get; private set; }
        public decimal Price { get; private set; }
        public Cart? Cart { get; private set; }
        public EventSeat? EventSeat { get; private set; }
        public DateTime AddedAtUtc { get; private set; }

        private CartItem() { }
        public CartItem(Guid cartId, Guid eventSeatId, decimal price)
        {
            if (cartId == Guid.Empty)
                throw new ArgumentException("cart id cannot be null!");

            if (eventSeatId == Guid.Empty)
                throw new ArgumentException("event seat id cannot be null!");

            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero.", nameof(price));

            CartId = cartId;
            EventSeatId = eventSeatId;
            Price = price;
            AddedAtUtc = DateTime.UtcNow;
        }
    }
}
