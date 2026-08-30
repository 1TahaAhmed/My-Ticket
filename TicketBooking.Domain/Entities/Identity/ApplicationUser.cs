using Microsoft.AspNetCore.Identity;
using System;

namespace TicketBooking.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public DateTime CreatedAtUtc { get; private set; }
        public bool IsActive { get; private set; }

        private ApplicationUser() { }

        public ApplicationUser(string firstName, string lastName, string email, string userName)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.", nameof(email));

            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("Username is required.", nameof(userName));

            SetFirstName(firstName);
            SetLastName(lastName);

            Id = Guid.NewGuid();
            Email = email.Trim();
            UserName = userName.Trim();
            CreatedAtUtc = DateTime.UtcNow;
            IsActive = true;
        }

        public void UpdateName(string newFirstName, string newLastName)
        {
            SetFirstName(newFirstName);
            SetLastName(newLastName);
        }

        public void Deactivate()
        {
            if (!IsActive) return;
            IsActive = false;
        }

        public void Activate()
        {
            if (IsActive) return;
            IsActive = true;
        }

        private void SetFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is required.", nameof(firstName));

            if (firstName.Length > 50)
                throw new ArgumentException("First name cannot exceed 50 characters.", nameof(firstName));

            FirstName = firstName.Trim();
        }

        private void SetLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name is required.", nameof(lastName));

            if (lastName.Length > 50)
                throw new ArgumentException("Last name cannot exceed 50 characters.", nameof(lastName));

            LastName = lastName.Trim();
        }
    }
}