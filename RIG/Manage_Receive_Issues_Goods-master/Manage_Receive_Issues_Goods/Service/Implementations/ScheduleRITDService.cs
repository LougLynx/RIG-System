using System.Collections.Generic;
using System.Threading.Tasks;
using Manage_Receive_Issues_Goods.DTO;
using Manage_Receive_Issues_Goods.Models;
using Manage_Receive_Issues_Goods.Repository;
using Microsoft.EntityFrameworkCore;


namespace Manage_Receive_Issues_Goods.Service.Implementations
{
    public class ScheduleRITDService : IScheduleRITDService
    {
        private readonly IScheduleRITDRepository _scheduleRepository;

        public ScheduleRITDService(IScheduleRITDRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task<IEnumerable<Planritddetail>> GetAllPlanDetailsAsync()
        {
            return await _scheduleRepository.GetAllPlanDetailsAsync();
        }

        public async Task<IEnumerable<Status>> GetAllStatusesAsync()
        {
            return await _scheduleRepository.GetAllStatusesAsync();
        }


        public async Task<IEnumerable<Planritddetail>> GetPlanDetails(int planId)
        {
            return await _scheduleRepository.GetPlanDetails(planId);
        }

        public async Task<Planritddetail> GetPlanDetailByIdAsync(int detailId)
        {
            return await _scheduleRepository.GetPlanDetailByIdAsync(detailId);
        }

        public async Task UpdatePlanDetailAsync(Planritddetail detail)
        {
            await _scheduleRepository.UpdatePlanDetailAsync(detail);
        }
        public async Task AddActualAsync(Actualsritd actual)
        {
            await _scheduleRepository.AddActualAsync(actual);
        }

        public async Task DeleteActualAsync(int actualId)
        {
            await _scheduleRepository.DeleteActualAsync(actualId);
        }
        public async Task AddPlanAsync(Planritd plan)
        {
            await _scheduleRepository.AddPlanAsync(plan);
        }

        public async Task AddPlanDetailAsync(Planritddetail planDetail)
        {
            await _scheduleRepository.AddPlanDetailAsync(planDetail);
        }
        public async Task<int> GetPlanIdByDetailsAsync(string planName, string planType, DateOnly effectiveDate)
        {
            return await _scheduleRepository.GetPlanIdByDetailsAsync(planName, planType, effectiveDate);
        }

		public async Task<(Planritd currentPlan, Planritd? nextPlan)> GetCurrentAndNextPlanAsync()
		{
			var currentPlan = await _scheduleRepository.GetCurrentPlanAsync();
			var nextPlan = await _scheduleRepository.GetNextPlanAsync();

			// Nếu không có kế hoạch kế tiếp, gán nextPlan là null để frontend biết cần lặp vô hạn.
			return (currentPlan, nextPlan);
		}

		public async Task<IEnumerable<PlanDetailDTO>> GetPlanDetailsForDisplayAsync()
		{
			var (currentPlan, nextPlan) = await GetCurrentAndNextPlanAsync();

			var startDate = currentPlan?.EffectiveDate ?? DateOnly.FromDateTime(DateTime.Today);
			var endDate = nextPlan?.EffectiveDate.AddDays(-1) ?? DateOnly.FromDateTime(DateTime.Today).AddYears(100); // Nếu không có kế hoạch tiếp theo, lặp lại trong 100 năm

			var planDetails = await _scheduleRepository.GetPlanDetailsBetweenDatesAsync(startDate, endDate);
			return planDetails.Select(pd => new PlanDetailDTO
			{
				PlanDetailId = pd.PlanDetailId,
				PlanTime = pd.PlanTime,
				PlanDetailName = pd.PlanDetailName,
				Actuals = pd.Actualsritds.Select(a => new ActualDetailDTO
				{
					ActualId = a.ActualId,
					PlanDetailId = a.PlanDetailId,
					ActualTime = a.ActualTime
				}).ToList()
			});
		}

        public async Task<IEnumerable<PlanDTO>> GetFuturePlansAsync()
        {
            var futurePlans = await _scheduleRepository.GetFuturePlansAsync();
            return futurePlans.Select(plan => new PlanDTO
            {
                PlanId = plan.PlanId,
                EffectiveDate = plan.EffectiveDate,
                TotalShipment = plan.TotalShipment,
                PlanDetails = plan.Planritddetails.Select(detail => new PlanDetailDTO
                {
                    PlanDetailId = detail.PlanDetailId,
                    PlanDetailName = detail.PlanDetailName,
                    PlanTime = detail.PlanTime
                }).ToList()
            }).ToList();
        }
        public async Task<IEnumerable<PlanDetailDTO>> GetPastPlanDetailsAsync()
        {
			var pastPlanDetails = await _scheduleRepository.GetPastPlanDetailsAsync();
			return pastPlanDetails.Select(pd => new PlanDetailDTO
			{
				PlanDetailId = pd.PlanDetailId,
				PlanTime = pd.PlanTime,
				PlanDetailName = pd.PlanDetailName,
				Actuals = pd.Actualsritds.Select(a => new ActualDetailDTO
				{
					ActualId = a.ActualId,
					PlanDetailId = a.PlanDetailId,
					ActualTime = a.ActualTime
				}).ToList()
			}).ToList();
		}
    }
}
