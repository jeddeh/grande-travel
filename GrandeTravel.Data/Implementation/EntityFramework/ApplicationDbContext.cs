using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using GrandeTravel.Entity;

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
