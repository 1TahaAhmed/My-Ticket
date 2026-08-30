using System;
using System.Collections.Generic;
using TicketBooking.Domain.BaseEntity;
using TicketBooking.Domain.Entities.Identity;
using TicketBooking.Domain.Enums;

namespace TicketBooking.Domain.Entities.Support
{
    public class SupportTicket : MBaseEntity
    {
        public Guid UserId { get; private set; }
        public ApplicationUser? User { get; private set; }

        public string TicketNumber { get; private set; } = string.Empty;
        public string Subject { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;

        public SupportTicketPriority Priority { get; private set; } = SupportTicketPriority.Medium;
        public SupportTicketStatus Status { get; private set; } = SupportTicketStatus.Open;

        public DateTime SubmittedAtUtc { get; private set; } = DateTime.UtcNow;
        public DateTime? ResolvedAtUtc { get; private set; }

        private SupportTicket() { }

        public SupportTicket(Guid userId, string subject, string description, SupportTicketPriority priority = SupportTicketPriority.Medium)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId is required.", nameof(userId));

            if (string.IsNullOrWhiteSpace(subject))
                throw new ArgumentException("Subject is required.", nameof(subject));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description is required.", nameof(description));

            UserId = userId;
            Subject = subject.Trim();
            Description = description.Trim();
            Priority = priority;
            Status = SupportTicketStatus.Open;
            SubmittedAtUtc = DateTime.UtcNow;
            TicketNumber = GenerateTicketNumber();
        }

        public void ChangeStatus(SupportTicketStatus newStatus)
        {
            Status = newStatus;
            if (newStatus == SupportTicketStatus.Resolved || newStatus == SupportTicketStatus.Closed)
            {
                ResolvedAtUtc = DateTime.UtcNow;
            }
        }

        public void ChangePriority(SupportTicketPriority newPriority)
        {
            Priority = newPriority;
        }

        private static string GenerateTicketNumber()
        {
            return $"SUP-{DateTime.UtcNow:yyMMdd}-{Guid.NewGuid().ToString()[..5].ToUpper()}";
        }
    }
}