using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TicketBooking.Domain.BaseEntity;

namespace TicketBooking.Domain.Entities.Venues
{
    public class VenueSection : BaseEntity<Guid>
    {
        [Required, ForeignKey(nameof(VenueZone))]
        public Guid VenueZoneId { get; private set; }
        public VenueZone? VenueZone { get; private set; }

        [Required, MaxLength(100)]
        public string Name { get; private set; } = string.Empty;
        [Required, MaxLength(100)]
        public string Code { get; private set; } = string.Empty;
        public int Capacity { get; private set; }

        private readonly List<Seat> _seats = new();
        public IReadOnlyCollection<Seat> Seats => _seats.AsReadOnly();

        private VenueSection() { }

        public VenueSection(
            Guid venueZoneId,
            string name,
            int capacity,
            string code = "")
        {
            VenueZoneId = venueZoneId;
            UpdateDetails(name, capacity, code);
        }

        public void UpdateDetails(string name, int capacity, string code = "")
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Section name cannot be empty.", nameof(name));

            if (capacity <= 0)
                throw new ArgumentException("Section capacity must be greater than zero.", nameof(capacity));

            Name = name.Trim();
            Capacity = capacity;
            Code = string.IsNullOrWhiteSpace(code)
                ? Name.Replace(" ", "").ToUpperInvariant()
                : code.Trim().ToUpperInvariant();
        }

        public void AddSeat(Seat seat)
        {
            ArgumentNullException.ThrowIfNull(seat);

            if (_seats.Count >= Capacity)
                throw new InvalidOperationException($"Cannot add more seats than Section capacity ({Capacity}).");

            if (_seats.Any(s => s.RowNumber == seat.RowNumber && s.SeatNumber == seat.SeatNumber))
                throw new InvalidOperationException($"Seat '{seat.RowNumber}-{seat.SeatNumber}' already exists in this Section.");

            _seats.Add(seat);
        }
    }
}