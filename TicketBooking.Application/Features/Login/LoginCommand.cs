using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TicketBooking.Application.Common.Models;

namespace TicketBooking.Application.Features.Login
{
    public record LoginCommand(
        string Email,
        string Password
        ) : IRequest<Result<LoginResponse>>;
}
