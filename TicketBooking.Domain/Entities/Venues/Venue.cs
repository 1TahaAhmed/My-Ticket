using System;
using System.Collections.Generic;
using System.Linq;
using TicketBooking.Domain.BaseEntity;
using TicketBooking.Domain.Entities.Catalog;

namespace TicketBooking.Domain.Entities.Venues
{
    public class Venue : BaseEntity<int>
    {
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string Address { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string Country { get; private set; } = string.Empty;
        public int Capacity { get; private set; }
        public string LocationUrl { get; private set; } = string.Empty;

        private readonly List<string> _gates = new();
        public IReadOnlyCollection<string> Gates => _gates.AsReadOnly();

        private readonly List<VenueZone> _zones = new();
        public IReadOnlyCollection<VenueZone> Zones => _zones.AsReadOnly();

        private Venue() { }

        public Venue(
            string name,
            string city,
            int capacity,
            string address = "",
            string country = "",
            string description = "",
            string locationUrl = "")
        {
            UpdateDetails(name, city, capacity, address, country, description, locationUrl);
        }

        public void UpdateDetails(
            string name,
            string city,
            int capacity,
            string address = "",
            string country = "",
            string description = "",
            string locationUrl = "")
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Venue name cannot be null or empty.", nameof(name));

            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City cannot be null or empty.", nameof(city));

            if (capacity <= 0)
                throw new ArgumentException("Capacity must be greater than zero.", nameof(capacity));

            Name = name.Trim();
            City = city.Trim();
            Capacity = capacity;
            Address = address?.Trim() ?? string.Empty;
            Country = country?.Trim() ?? string.Empty;
            Description = description?.Trim() ?? string.Empty;
            LocationUrl = locationUrl?.Trim() ?? string.Empty;
        }

        public void AddGate(string gate)
        {
            if (string.IsNullOrWhiteSpace(gate))
                throw new ArgumentException("Gate name cannot be empty.", nameof(gate));

            var trimmedGate = gate.Trim();
            if (_gates.Contains(trimmedGate, StringComparer.OrdinalIgnoreCase))
                throw new InvalidOperationException($"Gate '{trimmedGate}' already exists in this venue.");

            _gates.Add(trimmedGate);
        }

        public void RemoveGate(string gate)
        {
            if (string.IsNullOrWhiteSpace(gate)) return;
            _gates.RemoveAll(g => g.Equals(gate.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public void AddZone(VenueZone zone)
        {
            ArgumentNullException.ThrowIfNull(zone);

            if (_zones.Any(z => z.Id == zone.Id))
                throw new InvalidOperationException("This zone is already added to the venue.");

            _zones.Add(zone);
        }
    }
}