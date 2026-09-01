using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketBooking.Domain.Entities.Ticketing;

namespace TicketBooking.Infrastructure.Configurations
{
    public class RefundTransactionConfig : BaseEntityConfiguration<RefundTransaction, Guid>
    {
        public override void Configure(EntityTypeBuilder<RefundTransaction> builder)
        {
            base.Configure(builder);

            builder.ToTable("RefundTransactions");

            builder.Property(r => r.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(r => r.Reason)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(r => r.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(r => r.GatewayRefundReference)
                .HasMaxLength(100);

            builder.HasIndex(r => r.GatewayRefundReference);

            builder.Property(r => r.FailureReason)
                .HasMaxLength(500);

            builder.HasOne(r => r.PaymentTransaction)
                .WithMany()
                .HasForeignKey(r => r.PaymentTransactionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Booking)
                .WithMany()
                .HasForeignKey(r => r.BookingId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}