using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TicketBooking.Domain.BaseEntity
{
    public abstract class BaseEntity<TId>
    {
        [Key]
        public TId Id { get; protected set; } = default!;

        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        public string CreatedBy { get; protected set; } = string.Empty;

        public DateTime? LastModifiedDate { get; protected set; }
        public string LastModifiedBy { get; protected set; } = string.Empty;

        public bool IsDeleted { get; private set; }
        public DateTime? DeletedAt { get; protected set; }
        public string? DeletedBy { get; protected set; } = string.Empty;

        public void Remove(string deletedby)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = deletedby;
        }
    }
}
