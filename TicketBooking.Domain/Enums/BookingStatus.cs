using System;
using System.Collections.Generic;
using System.Text;

namespace TicketBooking.Domain.Enums
{
    public enum BookingStatus
    {
        none,
        Reserved,
        Confirmed,
        Expired,
        Cancelled
    }
}
