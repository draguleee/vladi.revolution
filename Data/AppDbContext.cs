﻿using Microsoft.EntityFrameworkCore;
using vladi.revolution.Models;

namespace vladi.revolution.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Player> Players { get; set; }  
    }
}