using System;
using System.Collections.Generic;
using System.Text;

namespace Ticket
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
