using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using MediatR;
using TicketBooking.Application.Common.Models;
using TicketBooking.Infrastructure.Interfaces;

namespace TicketBooking.Application.Features.Register
{
    public record RegisterCommand(
        string FirstName,
        string LastName,
        string UserName,
        string Email,
        string Password
    ) : IRequest<Result<string>>;
}
