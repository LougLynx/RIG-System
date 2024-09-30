using Microsoft.EntityFrameworkCore;
using Manage_Receive_Issues_Goods.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manage_Receive_Issues_Goods.Repository;

namespace Manage_Receive_Issues_Goods.Repositories.Implementations
{
    public class SchedulereceivedRepository : ISchedulereceivedRepository
    {
        private readonly RigContext _context;

        public SchedulereceivedRepository(RigContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Schedulereceived>> GetAllAsync()
        {
            return await _context.Schedulereceiveds
                .Include(s => s.Supplier)
                .Include(s => s.DeliveryTime)
                .Include(s => s.Weekday)
                .ToListAsync();
        }

        public async Task<Schedulereceived> GetByIdAsync(int id)
        {
            return await _context.Schedulereceiveds
                .Include(s => s.Supplier)
                .Include(s => s.DeliveryTime)
                .Include(s => s.Weekday)
                .FirstOrDefaultAsync(s => s.ScheduleId == id);
        }

        public async Task<IEnumerable<Schedulereceived>> GetSchedulesByWeekdayAsync(int weekdayId)
        {
            return await _context.Schedulereceiveds
                .Include(s => s.Supplier)
                .Include(s => s.DeliveryTime)
                .Include(s => s.Weekday)
                .Where(s => s.WeekdayId == weekdayId)
                .ToListAsync();
        }

        public async Task AddAsync(Schedulereceived entity)
        {
            await _context.Schedulereceiveds.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Schedulereceived entity)
        {
            _context.Schedulereceiveds.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Schedulereceiveds.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Actualreceived>> GetAllActualReceivedAsync()
        {
            return await _context.Actualreceiveds
                .Include(ar => ar.Schedule) // Bao gồm thông tin lịch trình liên quan
                .ThenInclude(s => s.Supplier) // Bao gồm cả nhà cung cấp
                .ToListAsync();
        }
    }
}
