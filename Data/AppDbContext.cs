using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using vladi.revolution.Models;

namespace vladi.revolution.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PlayerMatch>().HasKey(pm => new
            {
                pm.PlayerId,
                pm.MatchId
            });
            builder.Entity<PlayerMatch>().HasOne(m => m.Match).WithMany(pm => pm.PlayersMatches).HasForeignKey(m => m.MatchId);
            builder.Entity<PlayerMatch>().HasOne(m => m.Player).WithMany(pm => pm.PlayersMatches).HasForeignKey(m => m.PlayerId);
            base.OnModelCreating(builder);
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<PlayerMatch> PlayersMatches { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<Accident> Accidents { get; set; }
        public DbSet<Staff> StaffMembers { get; set; }

    }
}
