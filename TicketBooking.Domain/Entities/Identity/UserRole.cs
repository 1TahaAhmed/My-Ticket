using Microsoft.AspNetCore.Identity;
using System;

namespace TicketBooking.Domain.Entities.Identity
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string? Description { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }

        private ApplicationRole() : base() { }

        public ApplicationRole(string roleName, string? description = null) : base(roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Role name cannot be empty.", nameof(roleName));

            Description = description?.Trim();
            CreatedAtUtc = DateTime.UtcNow;
        }

        public void UpdateDescription(string? description)
        {
            Description = description?.Trim();
        }
    }
}