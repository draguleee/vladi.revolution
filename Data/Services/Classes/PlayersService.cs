using Microsoft.EntityFrameworkCore;
using vladi.revolution.Data.Base;
using vladi.revolution.Data.Services.Interfaces;
using vladi.revolution.Data.ViewModels;
using vladi.revolution.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace vladi.revolution.Data.Services.Classes
{
    public class PlayersService : EntityBaseRepository<Player>, IPlayersService
    {
        private readonly AppDbContext _context;

        public PlayersService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Player>> GetAllPlayersWithTransfersAsync()
        {
            var players = await _context.Players
                .Include(p => p.Transfers)
                .ToListAsync();
            return players;
        }

        public async Task<Player> GetPlayerByIdAsync(int id)
        {
            return await _context.Players
                .Include(p => p.Transfers) 
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddNewPlayerAsync(NewPlayerVM data)
        {
            var newPlayer = new Player
            {
                ProfilePictureUrl = data.ProfilePictureUrl,
                FullName = data.FullName,
                BirthDate = data.BirthDate,
                Position = data.Position,
                ShirtNumber = data.ShirtNumber,
                FacebookAccount = data.FacebookAccount,
                InstagramAccount = data.InstagramAccount,
                Transfers = new List<Transfer>()
            };

            await _context.Players.AddAsync(newPlayer);
            await _context.SaveChangesAsync();
        }

        public async Task<NewPlayerVM> GetPlayerForEditAsync(int id)
        {
            var player = await GetByIdAsync(id);
            if (player == null) return null;

            return new NewPlayerVM
            {
                Id = player.Id,
                ProfilePictureUrl = player.ProfilePictureUrl,
                FullName = player.FullName,
                BirthDate = player.BirthDate,
                Position = player.Position,
                ShirtNumber = player.ShirtNumber,
                FacebookAccount = player.FacebookAccount,
                InstagramAccount = player.InstagramAccount,
                Transfers = player.Transfers 
            };
        }

        public async Task UpdatePlayerAsync(int id, NewPlayerVM data)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == id);
            if (player == null) return;

            player.ProfilePictureUrl = data.ProfilePictureUrl;
            player.FullName = data.FullName;
            player.BirthDate = data.BirthDate;
            player.Position = data.Position;
            player.ShirtNumber = data.ShirtNumber;
            player.FacebookAccount = data.FacebookAccount;
            player.InstagramAccount = data.InstagramAccount;

            _context.Players.Update(player);
            await _context.SaveChangesAsync();
        }
    }
}
