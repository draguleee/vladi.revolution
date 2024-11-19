using Microsoft.EntityFrameworkCore;
using vladi.revolution.Data.Base;
using vladi.revolution.Data.Services.Interfaces;
using vladi.revolution.Data.ViewModels;
using vladi.revolution.Models;

namespace vladi.revolution.Data.Services
{
    public class TransfersService : EntityBaseRepository<Transfer>, ITransfersService
    {
        private readonly AppDbContext _context;

        public TransfersService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transfer>> GetAllTransfersWithPlayersAsync()
        {
            var transfers = await _context.Transfers
                .Include(a => a.Player)
                .OrderBy(a => a.Id)
                .ToListAsync();
            int sequence = 1;
            foreach (var transfer in transfers)
            {
                transfer.TransferNumber = sequence++;
            }
            return transfers;
        }

        public async Task<Transfer> GetTransferByIdAsync(int id)
        {
            var transferDetails = await _context.Transfers
                .Include(p => p.Player)
                .FirstOrDefaultAsync(n => n.Id == id);
            return transferDetails;
        }

        public async Task<PlayersDropdownVM> GetNewTransferDropdownsValues()
        {
            var response = new PlayersDropdownVM()
            {
                Players = await _context.Players.OrderBy(n => n.FullName).ToListAsync()
            };
            return response;
        }

        public async Task AddNewTransferAsync(NewTransferVM data)
        {
            var newTransfer = new Transfer()
            {
                TransferNumber = data.TransferNumber,
                TransferDate = data.TransferDate,
                TransferFrom = data.TransferFrom,
                TransferTo = data.TransferTo,
                PlayerId = data.PlayerId
            };
            await _context.Transfers.AddAsync(newTransfer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTransferAsync(int id, NewTransferVM data)
        {
            var dbTransfer = await _context.Transfers.FirstOrDefaultAsync(n => n.Id == id);
            if (dbTransfer != null)
            {
                dbTransfer.TransferNumber = data.TransferNumber;
                dbTransfer.TransferDate = data.TransferDate;
                dbTransfer.TransferFrom = data.TransferFrom;
                dbTransfer.TransferTo = data.TransferTo;
                dbTransfer.PlayerId = data.PlayerId;
                await _context.SaveChangesAsync();
            }
            await _context.SaveChangesAsync();
        }     
    }
}
