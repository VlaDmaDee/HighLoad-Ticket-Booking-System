using Microsoft.EntityFrameworkCore;
using TicketMaster.API.Entities;

namespace TicketMaster.API.Data {
    public class AppDbContext:DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) {
        
        }
        public DbSet<TicketEvent> TicketEvents { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<TicketEvent>().Property<uint>("xmin").IsRowVersion();
            base.OnModelCreating(modelBuilder);
        }
    }
}
