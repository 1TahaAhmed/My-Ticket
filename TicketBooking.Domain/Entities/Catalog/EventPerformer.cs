using System;

namespace TicketBooking.Domain.ValueObjects
{
    public class EventPerformer
    {
        public string Name { get; }
        public string? Role { get; } 
        public string? ImageUrl { get; }

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