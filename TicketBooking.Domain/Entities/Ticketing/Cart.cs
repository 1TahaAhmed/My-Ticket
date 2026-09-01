using System;
using System.Collections.Generic;
using System.Linq;
using TicketBooking.Domain.BaseEntity;
using TicketBooking.Domain.Entities.Identity;

namespace TicketBooking.Domain.Entities.Ticketing
{
    public class Cart : MBaseEntity
    {
        public Guid UserId { get; private set; }
        public ApplicationUser? User { get; private set; }

        private readonly List<CartItem> _items = new();
        public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

        public decimal TotalAmount => _items.Sum(x => x.Price);

        private Cart() { }

        public Cart(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId is required.", nameof(userId));

            UserId = userId;
        }

        public void AddItem(CartItem item)
        {
            ArgumentNullException.ThrowIfNull(item);

            if (_items.Any(i => i.SeatId == item.SeatId))
                throw new InvalidOperationException("This seat is already added to the cart.");

            _items.Add(item);
        }

        public void RemoveItem(int seatId)
        {
            var item = _items.FirstOrDefault(i => i.SeatId == seatId);
            if (item != null)
            {
                _items.Remove(item);
            }
        }

        public void Clear()
        {
            _items.Clear();
        }
    }
}