using vladi.revolution.Data.Base;
using vladi.revolution.Data.ViewModels;
using vladi.revolution.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace vladi.revolution.Data.Services.Interfaces
{
    public interface ITransfersService : IEntityBaseRepository<Transfer>
    {
        Task<Transfer> GetTransferByIdAsync(int id);
        Task<PlayersDropdownVM> GetNewTransferDropdownsValues();
        Task AddNewTransferAsync(NewTransferVM data);
        Task UpdateTransferAsync(int id, NewTransferVM data);
        Task<IEnumerable<Transfer>> GetAllTransfersWithPlayersAsync();
    }
}
