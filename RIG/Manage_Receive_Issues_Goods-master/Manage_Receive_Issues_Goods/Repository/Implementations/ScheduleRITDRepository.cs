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

        public async Task<IEnumerable<Planritd>> GetAllPlansAsync()
        {
            return await _context.Planritds.ToListAsync();
        }

        /*public async Task<IEnumerable<Planritddetail>> GetAllPlanDetailsAsync()
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
        }*/
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
        public async Task DeleteOldActualsAsync()
        {
            var yesterday = DateTime.Today.AddDays(-1);
            var oldActuals = await _context.Actualsritds
                .Where(a => a.ActualTime.Date == yesterday)
                .ToListAsync();

            _context.Actualsritds.RemoveRange(oldActuals);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PlanDetailDTO>> GetPlanAndActualDetailsAsync()
        {
            var result = await (from plan in _context.Planritddetails
                                join actual in _context.Actualsritds
                                on plan.PlanDetailId equals actual.PlanDetailId into planActuals
                                from pa in planActuals.DefaultIfEmpty()
                                select new PlanDetailDTO
                                {
                                    PlanDetailId = plan.PlanDetailId,
                                    PlanTime = plan.PlanTime,
                                    PlanDetailName = plan.PlanDetailName,
                                    Actuals = pa != null ? new List<ActualDetailDTO>
                            {
                                new ActualDetailDTO
                                {
                                    ActualId = pa.ActualId,
                                    PlanDetailId = pa.PlanDetailId,
                                    ActualTime = pa.ActualTime
                                }
                            } : new List<ActualDetailDTO>()
                                }).ToListAsync();

            return result;
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
