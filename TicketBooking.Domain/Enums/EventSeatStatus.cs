using System;
using System.Collections.Generic;
using System.Text;

namespace TicketBooking.Domain.Enums
{
    public enum EventSeatStatus
    {
        None = 0,
        Available,
        Reserved,
        Sold,
        Blocked
    }
}
