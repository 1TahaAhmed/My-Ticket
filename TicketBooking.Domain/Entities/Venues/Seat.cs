using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using TicketBooking.Domain.BaseEntity;

namespace TicketBooking.Domain.Entities.Venues
{
    public class Seat : MBaseEntity
    {
        public Guid VenueSectionId { get; private set; }
        public VenueSection? VenueSection { get; private set; }
        public string RowNumber { get; private set; } = string.Empty;
        public int SeatNumber { get; private set; }
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
