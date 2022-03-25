using System;
using Microsoft.EntityFrameworkCore;

namespace Mission13Final.Models
{
    public class BowlerContext : DbContext
    {
        // constructor
        public BowlerContext(DbContextOptions<BowlerContext> options) : base(options)
        {
        }

        //public DbSet<Bowler> BE { get; set; }
        public DbSet<Bowler> Bowlers { get; set; }
        public DbSet<Team> Teams { get; set; }
    }
}
