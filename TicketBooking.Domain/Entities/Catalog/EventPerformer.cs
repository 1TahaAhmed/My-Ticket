using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketBooking.Domain.BaseEntity;
using TicketBooking.Domain.Entities.Catalog;

namespace TicketBooking.Domain.ValueObjects
{
    public class EventPerformer : BaseEntity<Guid>
    {
        [Required, MaxLength(100)]
        public string Name { get; private set; } = string.Empty;
        [MaxLength(100)]
        public string? Role { get; private set; }
        [MaxLength(1000)]
        public string? ImageUrl { get; private set; }

        [Required, ForeignKey(nameof(Event))]
        public Guid EventId { get; private set; }
        public Event? Event { get; private set; }

        private EventPerformer() {  }
        public EventPerformer(string name, string? role = null, string? imageUrl = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Performer name is required.", nameof(name));

            Name = name.Trim();
            Role = role?.Trim();
            ImageUrl = imageUrl?.Trim();
        }
    }
}