using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TicketBooking.Domain.BaseEntity;

namespace TicketBooking.Domain.Entities.Catalog
{
    public class EventOrganizer : BaseEntity<Guid>
    {
        [Required, MaxLength(255)]
        public string Name { get; private set; } = default!;
        [Required, EmailAddress]
        public string Email { get; private set; } = default!;
        [Required, Phone]
        public string PhoneNumber { get; private set; } = default!;
        [MaxLength(1000)]
        public string? LogoUrl { get; private set; }
        [MaxLength(2550)]
        public string? Bio { get; private set; }
        public bool IsVerified { get; private set; }
        
        private readonly List<Event> _events = new();
        public IReadOnlyCollection<Event> Events => _events.AsReadOnly();

        private EventOrganizer() { }

        public EventOrganizer(string name, string email, string phoneNumber, string? logoUrl = null, string? bio = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Organizer name is required.", nameof(name));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Organizer email is required.", nameof(email));

            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Organizer phone number is required.", nameof(phoneNumber));

            Name = name.Trim();
            Email = email.Trim().ToLowerInvariant();
            PhoneNumber = phoneNumber.Trim();
            LogoUrl = logoUrl?.Trim();
            Bio = bio?.Trim();
            IsVerified = false;
        }

        public void UpdateProfile(string name, string email, string phoneNumber, string? logoUrl, string? bio)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Organizer name is required.", nameof(name));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Organizer email is required.", nameof(email));

            Name = name.Trim();
            Email = email.Trim().ToLowerInvariant();
            PhoneNumber = phoneNumber.Trim();
            LogoUrl = logoUrl?.Trim();
            Bio = bio?.Trim();
        }

        public void Verify()
        {
            IsVerified = true;
        }

        public void RevokeVerification()
        {
            IsVerified = false;
        }
    }
}
