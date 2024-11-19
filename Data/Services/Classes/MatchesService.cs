using Microsoft.EntityFrameworkCore;
using vladi.revolution.Data.Base;
using vladi.revolution.Data.Services.Interfaces;
using vladi.revolution.Data.ViewModels;
using vladi.revolution.Models;

namespace vladi.revolution.Data.Services.Classes
{
    public class MatchesService : EntityBaseRepository<Match>, IMatchesService
    {
        private readonly AppDbContext _context;

        public MatchesService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddNewMatchAsync(NewMatchVM data)
        {
            var newMatch = new Match()
            {
                MatchDate = data.MatchDate,
                HomeTeam = data.HomeTeam,
                AwayTeam = data.AwayTeam,
                HomeTeamScore = data.HomeTeamScore,
                AwayTeamScore = data.AwayTeamScore
            };
            await _context.Matches.AddAsync(newMatch);
            await _context.SaveChangesAsync();
            foreach (var playerId in data.PlayerIds)
            {
                var newPlayerMatch = new PlayerMatch()
                {
                    MatchId = newMatch.Id,
                    PlayerId = playerId
                };
                await _context.PlayersMatches.AddAsync(newPlayerMatch);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Match> GetMatchByIdAsync(int id)
        {
            var matchDetails = await _context.Matches
                .Include(pm => pm.PlayersMatches).ThenInclude(p => p.Player)
                .FirstOrDefaultAsync(n => n.Id == id);
            return matchDetails;
        }

        public async Task<PlayersDropdownVM> GetPlayersDropdownsValues()
        {
            var response = new PlayersDropdownVM()
            {
                Players = await _context.Players.OrderBy(n => n.FullName).ToListAsync()
            };
            return response;
        }

        public async Task UpdateMatchAsync(NewMatchVM data)
        {
            var dbMatch = await _context.Matches.FirstOrDefaultAsync(n => n.Id == data.Id);
            if (dbMatch != null) 
            {
                dbMatch.MatchDate = data.MatchDate;
                dbMatch.HomeTeam = data.HomeTeam;
                dbMatch.AwayTeam = data.AwayTeam;
                dbMatch.HomeTeamScore = data.AwayTeamScore;
                dbMatch.AwayTeamScore = data.AwayTeamScore;
                await _context.SaveChangesAsync();
            };
            var existingPlayersDb = _context.PlayersMatches.Where(n => n.MatchId == data.Id).ToList();
            _context.PlayersMatches.RemoveRange(existingPlayersDb);
            await _context.SaveChangesAsync();
            foreach (var playerId in data.PlayerIds)
            {
                var newPlayerMatch = new PlayerMatch()
                {
                    MatchId = data.Id,
                    PlayerId = playerId
                };
                await _context.PlayersMatches.AddAsync(newPlayerMatch);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Match>> GetAllMatchesWithPlayersAsync()
        {
            return await _context.Matches
                .Include(m => m.PlayersMatches) 
                .ThenInclude(pm => pm.Player)     
                .ToListAsync();
        }

    }
}
