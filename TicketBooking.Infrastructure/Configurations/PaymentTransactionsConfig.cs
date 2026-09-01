using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TicketBooking.Domain.Entities.Payments;

namespace TicketBooking.Infrastructure.Configurations
{
    public class PaymentTransactionsConfig : BaseEntityConfiguration<PaymentTransaction, Guid>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<PaymentTransaction> builder)
        {
            base.Configure(builder);

            builder.ToTable("PaymentTransactions");

            builder.HasOne(b => b.Booking)
                .WithMany()
                .HasForeignKey(b => b.BookingId)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);
            
            builder.Property(b => b.TransactionRef)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(b => b.TransactionRef);

            builder.Property(b => b.PaymentMethod)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(b => b.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(b => b.TransactionStatus)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(b => b.FailureReason)
                .HasMaxLength(1000);
        }
    }
}
