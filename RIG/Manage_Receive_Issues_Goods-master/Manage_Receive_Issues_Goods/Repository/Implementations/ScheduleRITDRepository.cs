using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manage_Receive_Issues_Goods.DTO;
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
       
        public async Task<IEnumerable<Planritddetail>> GetAllPlanDetailsAsync()
        {
            // Lấy ra danh sách tất cả các kế hoạch
            var plans = await _context.Planritds
                .OrderBy(p => p.EffectiveDate)
                .ToListAsync();

            if (plans == null || !plans.Any())
            {
                return Enumerable.Empty<Planritddetail>();
            }

            var today = DateOnly.FromDateTime(DateTime.Now);

            // Tìm kế hoạch gần nhất
            var closestPlan = plans
                .Where(p => p.EffectiveDate <= today)
                .OrderByDescending(p => p.EffectiveDate)
                .FirstOrDefault();

            if (closestPlan != null)
            {
                //Trả về danh sách các chi tiết kế hoạch của kế hoạch gần nhất
                return await _context.Planritddetails
                    .Where(pd => pd.PlanId == closestPlan.PlanId)
                    .ToListAsync();
            }

            return Enumerable.Empty<Planritddetail>();
        }

     
        public async Task<IEnumerable<Status>> GetAllStatusesAsync()
        {
            return await _context.Statuses
                .Include(s => s.PlanDetail)
                .ToListAsync();
        }
        public async Task<IEnumerable<Planritddetail>> GetPlanDetails(int planId)
        {
            var planDetails = await _context.Planritddetails
                .Where(d => d.PlanId == planId)
                .ToListAsync();
            return planDetails;
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
    
        public async Task AddActualAsync(Actualsritd actual)
        {
            _context.Actualsritds.Add(actual);
            await _context.SaveChangesAsync();
        }

		public async Task DeleteActualAsync(int actualId)
		{
			var actual = await _context.Actualsritds.FindAsync(actualId);
			if (actual != null)
			{
				_context.Actualsritds.Remove(actual);
				await _context.SaveChangesAsync();
			}
		}

        public async Task AddPlanAsync(Planritd plan)
        {
            _context.Planritds.Add(plan);
            await _context.SaveChangesAsync();
        }

        public async Task AddPlanDetailAsync(Planritddetail planDetail)
        {
            _context.Planritddetails.Add(planDetail);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetPlanIdByDetailsAsync(string planName, string planType, DateOnly effectiveDate)
        {
            var plan = await _context.Planritds
                .FirstOrDefaultAsync(p => p.PlanName == planName && p.PlanType == planType && p.EffectiveDate == effectiveDate);

            if (plan != null)
            {
                return plan.PlanId;
            }

            return 0;  
        }
      
        public async Task<Planritd> GetCurrentPlanAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return await _context.Planritds
                .Where(p => p.EffectiveDate <= today)
                .OrderByDescending(p => p.EffectiveDate)
                .FirstOrDefaultAsync();
        }

        public async Task<Planritd> GetNextPlanAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return await _context.Planritds
                .Where(p => p.EffectiveDate > today)
                .OrderBy(p => p.EffectiveDate)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Planritddetail>> GetPlanDetailsBetweenDatesAsync(DateOnly startDate, DateOnly endDate)
        {
            return await _context.Planritddetails
                .Include(pd => pd.Actualsritds)
                .Where(pd => pd.Plan.EffectiveDate >= startDate && pd.Plan.EffectiveDate < endDate)
                .ToListAsync();
        }
        public async Task<IEnumerable<Planritd>> GetFuturePlansAsync()
        {
            return await _context.Planritds
                .Include(p => p.Planritddetails)
                .Where(p => p.EffectiveDate > DateOnly.FromDateTime(DateTime.Now))
                .ToListAsync();
        }
        public async Task<IEnumerable<Planritddetail>> GetPastPlanDetailsAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            return await _context.Planritddetails
                .Include(p => p.Actualsritds)
                .Where(p => p.Plan.EffectiveDate < today)
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
