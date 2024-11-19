using vladi.revolution.Data.Base;
using vladi.revolution.Data.ViewModels;
using vladi.revolution.Models;

namespace vladi.revolution.Data.Services.Interfaces
{
    public interface IMatchesService : IEntityBaseRepository<Match>
    {
        Task<Match> GetMatchByIdAsync(int id);
        Task<PlayersDropdownVM> GetPlayersDropdownsValues();
        Task AddNewMatchAsync(NewMatchVM data);
        Task UpdateMatchAsync(NewMatchVM data);
        Task<IEnumerable<Match>> GetAllMatchesWithPlayersAsync();
    }
}
