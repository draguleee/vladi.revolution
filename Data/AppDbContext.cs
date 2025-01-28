using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
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

            // Goal: Scorer relationship
            builder.Entity<Goal>()
                .HasOne(g => g.Scorer)
                .WithMany()
                .HasForeignKey(g => g.ScorerId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete if needed

            // Goal: Assister relationship (optional)
            builder.Entity<Goal>()
                .HasOne(g => g.Assister)
                .WithMany()
                .HasForeignKey(g => g.AssisterId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete if needed

            // Goal: Match relationship
            builder.Entity<Goal>()
                .HasOne(g => g.Match)
                .WithMany(m => m.Goals)
                .HasForeignKey(g => g.MatchId);

            base.OnModelCreating(builder);

        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<PlayerMatch> PlayersMatches { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<Accident> Accidents { get; set; }
        public DbSet<Staff> StaffMembers { get; set; }

    }
}
