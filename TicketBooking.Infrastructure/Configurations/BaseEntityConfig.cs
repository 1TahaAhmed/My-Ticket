using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TicketBooking.Domain.BaseEntity;

namespace TicketBooking.Infrastructure.Configurations
{           
    public class BaseEntityConfiguration<T, TId> : IEntityTypeConfiguration<T> where T : BaseEntity<TId>
    {       
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {   
            builder.HasKey(t => t.Id);
            
            builder.Property(p => p.CreatedBy)
                .HasMaxLength(50);

            builder.Property(e => e.LastModifiedBy)
                .HasMaxLength(50);

            builder.Property(e => e.DeletedBy)
                .HasMaxLength(50);

            builder.HasQueryFilter(e => !e.IsDeleted);
        }   
    }       
}           
            