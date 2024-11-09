using vladi.revolution.Data.Base;
using vladi.revolution.Data.ViewModels;
using vladi.revolution.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace vladi.revolution.Data.Services.Interfaces
{
    public interface IPlayersService : IEntityBaseRepository<Player>
    {
        Task<Player> GetPlayerByIdAsync(int id); 
        Task AddNewPlayerAsync(NewPlayerVM data); 
        Task UpdatePlayerAsync(int id, NewPlayerVM data); 
        Task<IEnumerable<Player>> GetAllPlayersWithTransfersAsync(); 
        Task<NewPlayerVM> GetPlayerForEditAsync(int id); 
    }
}
