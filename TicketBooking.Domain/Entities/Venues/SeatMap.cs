using System;
using Ticket;
using TicketBooking.Domain.BaseEntity;

namespace TicketBooking.Domain.Entities.Venues
{
    public class SeatMap : MBaseEntity
    {
        public int VenueId { get; private set; }
        public Venue? Venue { get; private set; }

        public string Name { get; private set; } = string.Empty;           // Main Stage Map, Concert Layout A
        public string LayoutJson { get; private set; } = string.Empty;     // JSON Data (Coordinates, shapes, scale)
        public string? SvgUrl { get; private set; }                       // رابط صورة الـ SVG للمخطط البصري
        public bool IsActive { get; private set; } = true;

        private SeatMap() { }

        public SeatMap(int venueId, string name, string layoutJson, string? svgUrl = null)
        {
            if (venueId <= 0)
                throw new ArgumentException("VenueId is required.", nameof(venueId));

            VenueId = venueId;
            UpdateLayout(name, layoutJson, svgUrl);
            IsActive = true;
        }

        public void UpdateLayout(string name, string layoutJson, string? svgUrl = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Map name cannot be empty.", nameof(name));

            if (string.IsNullOrWhiteSpace(layoutJson))
                throw new ArgumentException("Layout JSON data cannot be empty.", nameof(layoutJson));

            Name = name.Trim();
            LayoutJson = layoutJson.Trim();
            SvgUrl = svgUrl?.Trim();
        }

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;
    }
}