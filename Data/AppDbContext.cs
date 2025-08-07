using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Venue> Venue { get; set; } = default!;

public DbSet<Event> Event { get; set; } = default!;

public DbSet<Booking> Booking { get; set; } = default!;
    }
