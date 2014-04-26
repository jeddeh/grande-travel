﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using GrandeTravel.Entity;

namespace GrandeTravel.Data
{
    public class GrandeTravelDbContext : DbContext
    {
        public GrandeTravelDbContext()
        {
            Database.SetInitializer<GrandeTravelDbContext>(null);
        }

        public GrandeTravelDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Database.SetInitializer<GrandeTravelDbContext>(null);
        }
        
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<TravelUser> TravelUsers { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Activity> Activities { get; set; }
    }
}