using System;
using TicketBooking.Domain.BaseEntity;

namespace TicketBooking.Domain.Entities.Ticketing
{
    public class CartItem : BaseEntity<Guid>
    {
        public Guid CartId { get; private set; }
        public Cart? Cart { get; private set; }

        public int SeatId { get; private set; }
        public decimal Price { get; private set; }
        public DateTime AddedAtUtc { get; private set; }

        private CartItem() { }

        public CartItem(Guid cartId, int seatId, decimal price)
        {
            if (cartId == Guid.Empty)
                throw new ArgumentException("Cart ID cannot be empty.", nameof(cartId));

            if (seatId <= 0)
                throw new ArgumentException("Seat ID must be a positive integer.", nameof(seatId));

            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero.", nameof(price));

            CartId = cartId;
            SeatId = seatId;
            Price = price;
            AddedAtUtc = DateTime.UtcNow;
        }
    }
}