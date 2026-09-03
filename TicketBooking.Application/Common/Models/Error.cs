using System;
using System.Collections.Generic;
using System.Text;

namespace TicketBooking.Application.Common.Models
{
    public record Error(string Code, string Description)
    {
        public static Error NotFound(string description) => new("NotFound", description);
        public static Error Validation(string description) => new("Validation", description);
        public static Error Unauthorized(string description) => new("Unauthorized", description);
        public static Error Forbidden(string description) => new("Forbidden", description);
        public static Error Conflict(string description) => new("Conflict", description);
        public static Error InternalServerError(string description) => new("InternalServerError", description);
    }
}
