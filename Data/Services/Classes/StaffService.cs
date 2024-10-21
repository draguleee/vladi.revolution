using Microsoft.EntityFrameworkCore;
using vladi.revolution.Data.Services.Interfaces;
using vladi.revolution.Models;

namespace vladi.revolution.Data.Services.Classes
{
    public class StaffService : IStaffService
    {
        private readonly AppDbContext _context;

        public StaffService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Staff>> GetAllAsync()
        {
            var result = await _context.StaffMembers.ToListAsync();
            return result;
        }

        public async Task AddAsync(Staff staffMember)
        {
            await _context.StaffMembers.AddAsync(staffMember);
            await _context.SaveChangesAsync();
        }

        public async Task<Staff> GetByIdAsync(int id)
        {
            var result = await _context.StaffMembers.FirstOrDefaultAsync(n => n.Id == id);
            return result;
        }

        public async Task<Staff> UpdateAsync(int id, Staff staffMember)
        {
            _context.Update(staffMember);
            await _context.SaveChangesAsync();
            return staffMember;   

        }

        public async Task DeleteAsync(int id)
        {
            var result = await _context.StaffMembers.FirstOrDefaultAsync(n => n.Id == id);
            _context.StaffMembers.Remove(result);
            await _context.SaveChangesAsync();
        }
    }
}
