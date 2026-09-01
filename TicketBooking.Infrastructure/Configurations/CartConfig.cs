using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TicketBooking.Domain.Entities.Ticketing;

namespace TicketBooking.Infrastructure.Configurations
{
    public class CartConfig : BaseEntityConfiguration<Cart, Guid>
    {
        public override void Configure(EntityTypeBuilder<Cart> builder)
        {
            base.Configure(builder);

            builder.ToTable("Carts");

            builder.HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(b => b.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Ignore(c => c.TotalAmount);

            builder.HasMany(c => c.Items)
                .WithOne(i => i.Cart)
                .HasForeignKey(i => i.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Metadata.FindNavigation(nameof(Cart.Items))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
