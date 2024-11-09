using vladi.revolution.Data.Base;
using vladi.revolution.Data.ViewModels;
using vladi.revolution.Models;

namespace vladi.revolution.Data.Services.Interfaces
{
    public interface IAccidentsService : IEntityBaseRepository<Accident>
    {
        Task<Accident> GetAccidentByIdAsync(int id);
        Task<PlayersDropdown> GetNewAccidentDropdownsValues();
        Task AddNewAccidentAsync(NewAccidentVM data);
        Task UpdateAccidentAsync(int id, NewAccidentVM data);
        Task<IEnumerable<Accident>> GetAllAccidentsWithPlayersAsync();
    }
}
