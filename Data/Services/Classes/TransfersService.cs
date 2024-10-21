using Microsoft.EntityFrameworkCore;
using vladi.revolution.Data.Services.Interfaces;
using vladi.revolution.Models;

namespace vladi.revolution.Data.Services.Classes
{
    public class TransfersService : ITransfersService
    {
        private readonly AppDbContext _context;

        public TransfersService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transfers>> GetAllAsync()
        {
            var result = await _context.Transfers.ToListAsync();
            return result;
        }

        public async Task AddAsync(Transfers transfers)
        {
            await _context.Transfers.AddAsync(transfers);
            await _context.SaveChangesAsync();
        }

        public async Task<Transfers> GetByIdAsync(int id)
        {
            var result = await _context.Transfers.FirstOrDefaultAsync(n => n.Id == id);
            return result;
        }

        public async Task<Transfers> UpdateAsync(int id, Transfers transfers)
        {
            _context.Update(transfers);
            await _context.SaveChangesAsync();
            return transfers;

        }

        public async Task DeleteAsync(int id)
        {
            var result = await _context.Transfers.FirstOrDefaultAsync(n => n.Id == id);
            _context.Transfers.Remove(result);
            await _context.SaveChangesAsync();
        }
    }
}
