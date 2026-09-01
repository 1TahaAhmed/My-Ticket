using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketBooking.Domain.Entities.Ticketing;

namespace TicketBooking.Infrastructure.Configurations
{
    public class CartItemConfig : BaseEntityConfiguration<CartItem, Guid>
    {
        public override void Configure(EntityTypeBuilder<CartItem> builder)
        {
            base.Configure(builder);

            builder.ToTable("CartItems");

            builder.Property(b => b.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasOne(b => b.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(b => b.CartId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}