using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TicketBooking.Domain.Entities.Ticketing;

namespace TicketBooking.Infrastructure.Configurations
{
    public class ResaleTransactionsConfig : BaseEntityConfiguration<ResaleTransaction, Guid>
    {
        public override void Configure(EntityTypeBuilder<ResaleTransaction> builder)
        {
            base.Configure(builder);

            builder.ToTable("ResaleTransactions");

            builder.HasOne(rt => rt.ResaleListing)
                   .WithMany()
                   .HasForeignKey(rt => rt.ResaleListingId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(rt => rt.Buyer)
                .WithMany()
                .HasForeignKey(rt => rt.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(rt => rt.Seller)
                .WithMany()
                .HasForeignKey(rt => rt.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(rt => rt.SaleAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(rt => rt.PlatformFee)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(rt => rt.SellerPayoutAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(rt => rt.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(rt => rt.PayoutStatus)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(rt => rt.GatewayTransactionReference)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(rt => rt.GatewayTransactionReference);

            builder.Property(rt => rt.PayoutTransactionReference)
                .HasMaxLength(100);
        
            builder.HasIndex(rt => rt.PayoutTransactionReference);
        }
    }
}
