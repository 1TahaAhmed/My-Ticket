using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Ticket;
using TicketBooking.Domain.BaseEntity;

namespace TicketBooking.Domain.Entities.Catalog
{    
    public class EventSchedule : BaseEntity<Guid>
    {
        [Required, ForeignKey(nameof(Event))]
        public Guid EventId { get; private set; }
        public Event? Event { get; private set; }
     
        public DateTime? DoorsOpenAtUtc { get; private set; }
        public DateTime StartAtUtc { get; private set; }
        public DateTime EndAtUtc { get; private set; }
     
        public bool IsCancelled { get; private set; }
     
        private EventSchedule() { }
        
        public EventSchedule(Guid eventId, DateTime startAtUtc, DateTime endAtUtc, DateTime? doorsOpenAtUtc = null)
        {
            EventId = eventId;
            SetSchedule(startAtUtc, endAtUtc, doorsOpenAtUtc);
        }

        public void SetSchedule(DateTime startAtUtc, DateTime endAtUtc, DateTime? doorsOpenAtUtc)
        {
            if (endAtUtc <= startAtUtc)
                throw new ArgumentException("End time must be after start time.", nameof(endAtUtc));

            if (doorsOpenAtUtc.HasValue && doorsOpenAtUtc.Value > startAtUtc)
                throw new ArgumentException("Doors open time cannot be after start time.", nameof(doorsOpenAtUtc));

            StartAtUtc = startAtUtc;
            EndAtUtc = endAtUtc;
            DoorsOpenAtUtc = doorsOpenAtUtc;
        }

        public void UpdateSchedule(DateTime startAtUtc , DateTime endAtUtc, DateTime doorsOpenAtUtc)
        {
            SetSchedule(startAtUtc, endAtUtc, doorsOpenAtUtc);
        }

        public void Cancel()
        {
            IsCancelled = true;
        }

        public void Restore()
        {
            IsCancelled = false;
        }
    }
}
