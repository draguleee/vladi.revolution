﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using vladi.revolution.Models;

namespace vladi.revolution.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Staff> StaffMembers { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<Accident> Accidents { get; set; }

    }
}
