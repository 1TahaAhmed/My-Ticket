using System;
using System.Collections.Generic;
using System.Text;

namespace TicketBooking.Domain.BaseEntity
{
    public abstract class MBaseEntity : BaseEntity<Guid>
    {
        protected MBaseEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}
