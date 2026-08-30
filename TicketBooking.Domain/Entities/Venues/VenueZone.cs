using System;
using System.Collections.Generic;
using System.Linq;
using Ticket;
using TicketBooking.Domain.BaseEntity;

namespace TicketBooking.Domain.Entities.Venues
{
    public class VenueZone : MBaseEntity
    {
        public Guid VenueId { get; private set; }
        public Venue? Venue { get; private set; }

        public string Name { get; private set; } = string.Empty;
        public string Code { get; private set; } = string.Empty;
        public int Capacity { get; private set; }
        public bool HasNumberedSeats { get; private set; }
        public string? GateName { get; private set; }

        private readonly List<VenueSection> _sections = new();
        public IReadOnlyCollection<VenueSection> Sections => _sections.AsReadOnly();

        private VenueZone() { }

        public VenueZone(
            Guid venueId,
            string name,
            string code,
            int capacity,
            bool hasNumberedSeats = true,
            string? gateName = null)
        {
            if (venueId == Guid.Empty)
                throw new ArgumentException("VenueId is required.", nameof(venueId));

            VenueId = venueId;
            HasNumberedSeats = hasNumberedSeats;
            GateName = gateName?.Trim();

            UpdateDetails(name, code, capacity);
        }

        public void UpdateDetails(string name, string code, int capacity)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Zone name cannot be empty.", nameof(name));

            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Zone code cannot be empty.", nameof(code));

            if (capacity <= 0)
                throw new ArgumentException("Capacity must be greater than zero.", nameof(capacity));

            Name = name.Trim();
            Code = code.Trim().ToUpperInvariant();
            Capacity = capacity;
        }

        public void AddSection(VenueSection section)
        {
            ArgumentNullException.ThrowIfNull(section);

            var currentTotalCapacity = _sections.Sum(s => s.Capacity);
            if (currentTotalCapacity + section.Capacity > Capacity)
                throw new InvalidOperationException($"Cannot add section. Exceeds Zone total capacity ({Capacity}).");

            _sections.Add(section);
        }
    }
}