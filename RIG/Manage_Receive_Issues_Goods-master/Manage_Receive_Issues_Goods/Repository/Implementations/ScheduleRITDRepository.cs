using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manage_Receive_Issues_Goods.Models;
using Microsoft.EntityFrameworkCore;

namespace Manage_Receive_Issues_Goods.Repository.Implementations
{
    public class ScheduleRITDRepository : IScheduleRITDRepository
    {
        private readonly RigContext _context;

        public ScheduleRITDRepository(RigContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Planritd>> GetAllPlansAsync()
        {
            return await _context.Planritds.ToListAsync();
        }

        public async Task<IEnumerable<Planritddetail>> GetAllPlanDetailsAsync()
        {
            var planDetails = await _context.Planritddetails.ToListAsync();
            var effectiveDate = planDetails.FirstOrDefault()?.Plan.EffectiveDate ?? DateOnly.FromDateTime(DateTime.Now);

            foreach (var detail in planDetails)
            {
                if (detail.PlanDate < effectiveDate)
                {
                    detail.PlanDate = effectiveDate;
                }
            }
                Console.WriteLine("Plan detail controller:" + planDetails);
            return planDetails;
        }

        public async Task<IEnumerable<Actualsritd>> GetAllActualsAsync()
        {
            return await _context.Actualsritds
                .Include(a => a.PlanDetail)
                .ToListAsync();
        }

        public async Task<IEnumerable<Status>> GetAllStatusesAsync()
        {
            return await _context.Statuses
                .Include(s => s.PlanDetail)
                .ToListAsync();
        }

        public async Task<IEnumerable<Planritddetail>> GetPlanDetailsForWeekAsync()
        {
            var startOfWeek = DateOnly.FromDateTime(DateTime.Now.StartOfWeek(DayOfWeek.Monday));
            var endOfWeek = startOfWeek.AddDays(7);

            return await _context.Planritddetails
                .Include(p => p.Plan)
                .Where(p => p.PlanDate >= startOfWeek && p.PlanDate < endOfWeek)
                .ToListAsync();
        }

        public async Task<Planritddetail> GetPlanDetailByIdAsync(int detailId)
        {
            return await _context.Planritddetails
                .FirstOrDefaultAsync(d => d.PlanDetailId == detailId);
        }

        public async Task UpdatePlanDetailAsync(Planritddetail detail)
        {
            _context.Planritddetails.Update(detail);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Planritddetail>> GetAllPlanDetailsWithoutDateAsync()
        {
            return await _context.Planritddetails
                .Select(p => new Planritddetail
                {
                    PlanDetailId = p.PlanDetailId,
                    PlanTime = p.PlanTime,
                    PlanDetailName = p.PlanDetailName
                })
                .ToListAsync();
        }
    }

    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}
