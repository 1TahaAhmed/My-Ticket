using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml.Linq;
using TicketBooking.Domain.BaseEntity;
using TicketBooking.Domain.Entities.Venues;

namespace TicketBooking.Domain.Entities.Venues
{
    public class Seat : BaseEntity<int>
    {
        [Required, ForeignKey(nameof(VenueSection))]
        public Guid VenueSectionId { get; private set; }
        public VenueSection? VenueSection { get; private set; }

        [Required, MaxLength(1000)]
        public string RowNumber { get; private set; } = string.Empty;
        public int SeatNumber { get; private set; }
        [Required, MaxLength(1500)]
        public string Label => $"{RowNumber}-{SeatNumber}";
        public bool IsAccessible { get; private set; }
        public bool IsRestrictedView { get; private set; }

        private Seat() { }
        public Seat(Guid venueSectionId,
            string rowNumber,
            int seatNumber,
            string label)
        {
            VenueSectionId = venueSectionId;
            RowNumber = rowNumber;
            SeatNumber = seatNumber;

            UpdateSeat(rowNumber, seatNumber, label);
        }

        public void UpdateSeat(string rowNumber, int seatNumber, string label)
        {
            if (string.IsNullOrWhiteSpace(rowNumber))
                throw new ArgumentException("Row number cannot be empty.", nameof(rowNumber));
            
            if (seatNumber <= 0)
                throw new ArgumentException("Seat Number cannot be empty.", nameof(seatNumber));
            
            RowNumber = rowNumber;
            SeatNumber = seatNumber;
        }

        public void SetAccessibility(bool isAccessibile) => IsAccessible = isAccessibile;
        public void SetRestrictedView(bool isRestrictedView) => IsRestrictedView = isRestrictedView;
    }
}
