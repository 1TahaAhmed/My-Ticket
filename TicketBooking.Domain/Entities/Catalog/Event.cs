using System;
using System.Collections.Generic;
using System.Text;
using Ticket;
using TicketBooking.Domain.BaseEntity;

namespace TicketBooking.Domain.Entities.Catalog
{
    public class Event : MBaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string Slug { get; private set; } = string.Empty;
        public string Metadata { get; private set; } = string.Empty;
        public EventStatus Status { get; private set; } = EventStatus.Draft;

        public Guid VenueId { get; private set; }
        public Venue? Venue { get; private set; }
        public int CategoryId { get; private set; }
        public Category? Category { get; private set; }
        public Guid OrganizerId { get; private set; }
        public EventOrganizer? Organizer { get; private set; }

        private readonly List<EventSchedule> _schedules = new();
        public IReadOnlyCollection<EventSchedule> Schedules => _schedules.AsReadOnly();

        private readonly List<EventPerformer> _performers = new();
        public IReadOnlyCollection<EventPerformer> Performers => _performers.AsReadOnly();

        private Event() { }
        public Event(string name, int categoryId, Guid venueId, Guid organizerId, string description = "")
        {
            UpdateDetails(name, description);

            if (categoryId <= 0) throw new ArgumentException("Invalid CategoryId", nameof(categoryId));
            CategoryId = categoryId;

            VenueId = venueId != Guid.Empty ? venueId : throw new ArgumentException("Invalid VenueId", nameof(venueId));
            OrganizerId = organizerId != Guid.Empty ? organizerId : throw new ArgumentException("Invalid OrganizerId", nameof(organizerId));
        }

        public void UpdateDetails(string name, string description)
        {
            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Event name can't be null", nameof(name));

            Name = name.Trim();
            Description = description?.Trim() ?? string.Empty;

            Slug = Name.ToLowerInvariant().Replace(" ", "-");
        }

        public void AddSchedule(EventSchedule schedule)
        {
            ArgumentNullException.ThrowIfNull(schedule);
            _schedules.Add(schedule);
        }

        public void AddPerformer(EventPerformer performer) 
        {
            ArgumentNullException.ThrowIfNull(performer);
            _performers.Add(performer);
        }

        public void Publish()
        {
            if(_schedules.Count == 0)
                throw new InvalidOperationException("Cannot publish an event without at least one schedule.");

            Status = EventStatus.Published;
        }

        public void Cancel()
        {
            Status = EventStatus.Cancelled;
        }
    }
}