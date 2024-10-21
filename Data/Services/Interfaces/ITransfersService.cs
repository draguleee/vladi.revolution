using vladi.revolution.Models;

namespace vladi.revolution.Data.Services.Interfaces
{
    public interface ITransfersService
    {
        Task<IEnumerable<Transfers>> GetAllAsync();
        Task<Transfers> GetByIdAsync(int id);
        Task AddAsync(Transfers transfers);
        Task<Transfers> UpdateAsync(int id, Transfers transfer);
        Task DeleteAsync(int id);
    }
}
