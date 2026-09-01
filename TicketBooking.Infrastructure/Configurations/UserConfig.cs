using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TicketBooking.Domain.Entities.Identity;

namespace TicketBooking.Infrastructure.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users");

            builder.Property(b => b.Email)
                .HasMaxLength(200)
                .IsRequired();

            builder.HasIndex(b => b.Email)
                .IsUnique();

            builder.Property(b => b.FirstName)
                .IsRequired();

            builder.Property(b => b.LastName)
                .IsRequired();

            builder.Property(b => b.UserName)
                .HasMaxLength(200)
                .IsRequired();

            builder.HasIndex(b => b.UserName)
                .IsUnique();

            builder.Property(b => b.PhoneNumber)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
