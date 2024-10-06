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
            // Lấy tất cả kế hoạch xếp theo EffectiveDate giảm dần
            var plans = await _context.Planritds
                .OrderByDescending(p => p.EffectiveDate)
                .ToListAsync();

            if (plans == null || !plans.Any())
            {
                return Enumerable.Empty<Planritddetail>();
            }
            
            var firstPlan = plans.First();
            var today = DateOnly.FromDateTime(DateTime.Now);

            //Xét xem kế hoạch đầu tiên có EffectiveDate lớn hơn hoặc bằng ngày hiện tại không
            if (firstPlan.EffectiveDate >= today)
            {
                //Nếu EffectiveDate của kế hoạch đầu tiên bằng ngày hiện tại
                if (firstPlan.EffectiveDate == today)
                {
                    //Lấy luôn kế hoạch đó
                    return await _context.Planritddetails
                        .Where(pd => pd.PlanId == firstPlan.PlanId)
                        .ToListAsync();
                }
                else
                {
                    //Nếu EffectiveDate của kế hoạch đầu tiên lớn hơn ngày hiện tại thì lấy kế hoạch thứ 2
                    var secondPlan = plans.Skip(1).FirstOrDefault();
                    if (secondPlan != null)
                    {
                        return await _context.Planritddetails
                            .Where(pd => pd.PlanId == secondPlan.PlanId)
                            .ToListAsync();
                    }
                }
            }

            //Nếu không kế hoạch đầu tiên có EffectiveDate nhỏ ngày hiện tại thì là kế hoạch hiện tại
            return await _context.Planritddetails
                .Where(pd => pd.PlanId == firstPlan.PlanId)
                .ToListAsync();
        }


        public async Task<IEnumerable<Actualsritd>> GetAllActualsAsync()
        {
            return await _context.Actualsritds
                .Include(a => a.PlanDetail)
                .ToListAsync();

            //return await _context.Actualsritds.ToListAsync();
        }

        public async Task<IEnumerable<Status>> GetAllStatusesAsync()
        {
            return await _context.Statuses
                .Include(s => s.PlanDetail)
                .ToListAsync();
        }

        /*public async Task<IEnumerable<Planritddetail>> GetPlanDetailsForWeekAsync()
        {
            var startOfWeek = DateOnly.FromDateTime(DateTime.Now.StartOfWeek(DayOfWeek.Monday));
            var endOfWeek = startOfWeek.AddDays(7);

            return await _context.Planritddetails
                //.Include(p => p.Plan)
                .Where(p => p.PlanDate >= startOfWeek && p.PlanDate < endOfWeek)
                .ToListAsync();
        }*/

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
