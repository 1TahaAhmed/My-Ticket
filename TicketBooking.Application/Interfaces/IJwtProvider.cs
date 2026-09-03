using System;
using System.Collections.Generic;
using System.Text;
using TicketBooking.Domain.Entities.Identity;

namespace TicketBooking.Infrastructure.Interfaces
{
    public interface IJwtProvider
    {
        string Generate(ApplicationUser user, IEnumerable<string>? roles = null);
    }
}
