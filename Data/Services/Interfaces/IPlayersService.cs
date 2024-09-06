using vladi.revolution.Models;

namespace vladi.revolution.Data.Services.Interfaces
{
    public interface IPlayersService
    {
        Task<IEnumerable<Player>> GetAllAsync();
        Task<Player> GetByIdAsync(int id);
        Task AddAsync(Player player);
        Task<Player> UpdateAsync(int id, Player player);
        Task DeleteAsync(int id);
    }
}
