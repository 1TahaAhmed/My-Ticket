using System;
using System.Collections.Generic;
using System.Text;

namespace TicketBooking.Domain.Enums
{
    public enum TicketStatus
    {
        none = 0,
        Issued,
        Used, 
        Cancelled,
        Refunded
    }
}
