using vladi.revolution.Models;

namespace vladi.revolution.Data.Services.Interfaces
{
    public interface IStaffService
    {
        Task<IEnumerable<Staff>> GetAllAsync();
        Task<Staff> GetByIdAsync(int id);
        Task AddAsync(Staff staffMember);
        Task<Staff> UpdateAsync(int id, Staff staffMember);
        Task DeleteAsync(int id);
    }
}
