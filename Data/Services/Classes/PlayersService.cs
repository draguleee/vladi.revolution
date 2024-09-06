using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using vladi.revolution.Data.Services.Interfaces;
using vladi.revolution.Models;

namespace vladi.revolution.Data.Services.Classes
{
    public class PlayersService : IPlayersService
    {
        private readonly AppDbContext _context;

        public PlayersService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Player>> GetAllAsync()
        {
            var result = await _context.Players.ToListAsync();
            return result;
        }

        public async Task AddAsync(Player player)
        {
            await _context.Players.AddAsync(player);
            await _context.SaveChangesAsync();
        }

        public async Task<Player> GetByIdAsync(int id)
        {
            var result = await _context.Players.FirstOrDefaultAsync(n => n.Id == id);
            return result;
        }

        public async Task<Player> UpdateAsync(int id, Player newPlayer)
        {
            _context.Update(newPlayer);
            await _context.SaveChangesAsync();
            return newPlayer;

        }

        public async Task DeleteAsync(int id)
        {
            var result = await _context.Players.FirstOrDefaultAsync(n => n.Id == id);
            _context.Players.Remove(result);
            await _context.SaveChangesAsync();
        }
    }
}
