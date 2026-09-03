using System;
using System.Collections.Generic;
using System.Text;

namespace TicketBooking.Application.Features.Login
{
    public record LoginResponse(
        string Token,
        string Email,
        DateTime ExpiresAtUtc
    );
}
