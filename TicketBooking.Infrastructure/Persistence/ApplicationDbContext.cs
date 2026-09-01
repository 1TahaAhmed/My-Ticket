using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TicketBooking.Domain.Entities.Catalog;
using TicketBooking.Domain.Entities.Identity;
using TicketBooking.Domain.Entities.Payments;
using TicketBooking.Domain.Entities.Pricing;
using TicketBooking.Domain.Entities.Ticketing;
using TicketBooking.Domain.Entities.Venues;
using TicketBooking.Domain.ValueObjects;
namespace TicketBooking.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Catalog
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<EventOrganizer> EventOrganizers => Set<EventOrganizer>();
        public DbSet<EventPerformer> EventPerformers => Set<EventPerformer>();
        public DbSet<EventSchedule> EventSchedules => Set<EventSchedule>();

        // Identity
        public DbSet<ApplicationUser> Users => Set<ApplicationUser>();

        // Venues & Seating
        public DbSet<Venue> Venues => Set<Venue>();
        public DbSet<VenueZone> VenueZones => Set<VenueZone>();
        public DbSet<VenueSection> VenueSections => Set<VenueSection>();
        public DbSet<Seat> Seats => Set<Seat>();
        public DbSet<EventSeat> EventSeats => Set<EventSeat>();

        // Bookings, Cart & Tickets
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<BookingItem> BookingItems => Set<BookingItem>();
        public DbSet<TicketBooking.Domain.Entities.Ticketing.Ticket> Tickets => Set<TicketBooking.Domain.Entities.Ticketing.Ticket>();

        // Payments & Resale
        public DbSet<PaymentTransaction> PaymentTransactions => Set<PaymentTransaction>();
        public DbSet<ResaleTransaction> ResaleTransactions => Set<ResaleTransaction>();
        public DbSet<RefundTransaction> RefundTransactions => Set<RefundTransaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}