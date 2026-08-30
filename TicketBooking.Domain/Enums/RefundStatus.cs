using System;
using System.Collections.Generic;
using System.Text;

namespace TicketBooking.Domain.Enums
{
    public enum RefundStatus
    {
        none,
        Pending,
        Completed,
        Failed,
        Cancelled
    }
}
