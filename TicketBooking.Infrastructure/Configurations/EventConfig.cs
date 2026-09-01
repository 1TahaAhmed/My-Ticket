using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketBooking.Domain.Entities.Catalog;

namespace TicketBooking.Infrastructure.Configurations
{
    public class EventConfig : BaseEntityConfiguration<Event, Guid>
    {
        public override void Configure(EntityTypeBuilder<Event> builder)
        {
            base.Configure(builder);

            builder.ToTable("Events");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Description)
                .HasMaxLength(500);

            builder.Property(c => c.Slug)
                .HasMaxLength(50);

            builder.Property(c => c.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.HasMany(b => b.Venues)
                .WithMany()
                .UsingEntity(j => j.ToTable("EventVenues"));

            builder.Navigation(e => e.Venues)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(s => s.Schedules)
                .WithOne(e => e.Event)
                .HasForeignKey(s => s.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(e => e.Schedules)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(p => p.Performers)
                .WithOne(e => e.Event)
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(e => e.Performers)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}