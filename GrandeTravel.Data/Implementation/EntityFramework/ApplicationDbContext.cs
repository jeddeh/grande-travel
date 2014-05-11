using GrandeTravel.Entity;

using System.Data.Entity;

namespace GrandeTravel.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }

        public ApplicationDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }
        
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Activity> Activities { get; set; }
    }
}
