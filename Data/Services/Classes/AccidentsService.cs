using Microsoft.EntityFrameworkCore;
using vladi.revolution.Data.Base;
using vladi.revolution.Data.Services.Interfaces;
using vladi.revolution.Data.ViewModels;
using vladi.revolution.Models;

namespace vladi.revolution.Data.Services.Classes
{
    public class AccidentsService : EntityBaseRepository<Accident>, IAccidentsService
    {
        private readonly AppDbContext _context;

        public AccidentsService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Accident>> GetAllAccidentsWithPlayersAsync()
        {
            var accidents = await _context.Accidents
                .Include(a => a.Player)
                .OrderBy(a => a.Id)
                .ToListAsync();
            return accidents;
        }

        public async Task<Accident> GetAccidentByIdAsync(int id)
        {
            var accidentDetails = await _context.Accidents
                .Include(a => a.Player)
                .FirstOrDefaultAsync(n => n.Id == id);
            return accidentDetails;
        }

        public async Task<PlayersDropdownVM> GetNewAccidentDropdownsValues()
        {
            var response = new PlayersDropdownVM()
            {
                Players = await _context.Players.OrderBy(n => n.FullName).ToListAsync()
            };
            return response;
        }

        public async Task AddNewAccidentAsync(NewAccidentVM data)
        {
            var newAccident = new Accident()
            {
                AccidentFrom = data.AccidentFrom,
                AccidentTo = data.AccidentTo,
                AccidentType = data.AccidentType,
                PlayerId = data.PlayerId
            };
            await _context.Accidents.AddAsync(newAccident);
            await _context.SaveChangesAsync();  
        }

        public async Task UpdateAccidentAsync(int id, NewAccidentVM data)
        {
            var dbAccident = await _context.Accidents.FirstOrDefaultAsync(n => n.Id == id);
            if (dbAccident != null)
            {
                dbAccident.AccidentFrom = data.AccidentFrom;
                dbAccident.AccidentTo = data.AccidentTo;
                dbAccident.AccidentType = data.AccidentType;
                dbAccident.PlayerId = data.PlayerId;
                await _context.SaveChangesAsync();
            }
            await _context.SaveChangesAsync();
        }
    }
}
