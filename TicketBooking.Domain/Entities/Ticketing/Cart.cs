using System;
using System.Collections.Generic;
using System.Text;
using TicketBooking.Domain.BaseEntity;
using TicketBooking.Domain.Entities.Ticketing;
using System.Linq;

namespace TicketBooking.Domain.Entities.Ticketing
{
    public class Cart : MBaseEntity
    {
        public Guid UserId { get; private set; }
        public DateTime ExpiresAtUtc { get; private set; }
        public bool IsCheckedOut { get; private set; }

        private readonly List<CartItem> _items = new();
        public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

        public decimal TotalAmount => _items.Sum(x => x.Price);
        public bool IsExpired => DateTime.UtcNow > ExpiresAtUtc;

        private Cart() { }
        public Cart(Guid userId, int expirationMinutes = 15) 
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId is required.", nameof(userId));

            if (expirationMinutes <= 0)
                throw new ArgumentException("Expiration minutes must be greater than zero.", nameof(expirationMinutes));

            UserId = userId;
            ExpiresAtUtc = DateTime.UtcNow.AddMinutes(expirationMinutes);
            IsCheckedOut = false;
        }
        public void AddItem(CartItem item) 
        {
            ArgumentNullException.ThrowIfNull(item);

            if (IsCheckedOut)
                throw new InvalidOperationException("Cannot add items to a checked-out cart.");

            if (IsExpired)
                throw new InvalidOperationException("Cannot add items to an expired cart.");

            if (_items.Any(x => x.EventSeatId == item.EventSeatId))
                throw new InvalidOperationException("This seat is already in the cart.");

            _items.Add(item);
        }
        public void RemoveItem(Guid eventSeatId)
        {
            if (IsCheckedOut)
                throw new InvalidOperationException("Cannot remove items from a checked-out cart.");

            var item = _items.FirstOrDefault(x => x.EventSeatId == eventSeatId);
            if (item != null)
            {
                _items.Remove(item);
            }
        }
        public void MarkAsCheckedOut()
        {
            if (IsCheckedOut)
                throw new InvalidOperationException("Cart is already checked out.");

            if (IsExpired)
                throw new InvalidOperationException("Cannot check out an expired cart.");

            if (!_items.Any())
                throw new InvalidOperationException("Cannot check out an empty cart.");

            IsCheckedOut = true;
        }

        public void Clear()
        {
            if (IsCheckedOut)
                throw new InvalidOperationException("Cannot clear a checked-out cart.");

            _items.Clear();
        }
    }
}
