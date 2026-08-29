using System;
using System.Collections.Generic;
using System.Text;
using TicketBooking.Domain.BaseEntity;

namespace TicketBooking.Domain.Entities.Venues
{
    public class Venue : MBaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string Address { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string Country { get; private set; } = string.Empty;
        public int Capacity {  get; private set; }
        public string LocationUrl { get; private set; } = string.Empty;

        private readonly List<VenueGate> _gates = new();
        public IReadOnlyCollection<VenueGate> gates => _gates.AsReadOnly();
        

        private readonly List<VenueZone> _zones = new();
        public IReadOnlyCollection<VenueZone> zones => _zones.AsReadOnly();
        
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

        public void AddGate(VenueGate gate) 
        {
            ArgumentNullException.ThrowIfNull(gate);
            _gates.Add(gate);
        }
        public void AddZone(VenueZone zone) 
        {
            ArgumentNullException.ThrowIfNull(zone);
            _zones.Add(zone);
        }
    }
}
