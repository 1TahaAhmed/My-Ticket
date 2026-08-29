using System;
using System.Collections.Generic;
using System.Text;

namespace TicketBooking.Domain.Enums
{
    public enum EventStatus
    {
        none = 0,
        Draft = 1,
        Published = 2,
        Cancelled = 3,
        Completed = 4
    }
}
