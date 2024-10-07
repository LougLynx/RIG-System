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
        public async Task<IEnumerable<Actualsritd>> GetAllActualsAsync()
        {
            return await _scheduleRepository.GetAllActualsAsync();
        }

        public async Task<IEnumerable<Planritddetail>> GetAllPlanDetailsAsync()
        {
            return await _scheduleRepository.GetAllPlanDetailsAsync();
        }

        public async Task<IEnumerable<Planritd>> GetAllPlansAsync()
        {
            return await _scheduleRepository.GetAllPlansAsync();
        }

        public async Task<IEnumerable<Status>> GetAllStatusesAsync()
        {
            return await _scheduleRepository.GetAllStatusesAsync();
        }

        /*public async Task<IEnumerable<Planritddetail>> GetPlanDetailsForWeekAsync()
        {
            return await _scheduleRepository.GetPlanDetailsForWeekAsync();
        }*/

        public async Task<Planritddetail> GetPlanDetailByIdAsync(int detailId)
        {
            return await _scheduleRepository.GetPlanDetailByIdAsync(detailId);
        }

        public async Task UpdatePlanDetailAsync(Planritddetail detail)
        {
            await _scheduleRepository.UpdatePlanDetailAsync(detail);
        }
        public async Task<IEnumerable<Planritddetail>> GetAllPlanDetailsWithoutDateAsync()
        {
            return await _scheduleRepository.GetAllPlanDetailsWithoutDateAsync();
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

        public async Task DeleteOldActualsAsync()
        {
            await _scheduleRepository.DeleteOldActualsAsync();
        }
        public async Task<IEnumerable<PlanDetailDTO>> GetPlanAndActualDetailsAsync()
        {
            return await _scheduleRepository.GetPlanAndActualDetailsAsync();
        }

        public async Task<IEnumerable<PlanDetailDTO>> GetPlanDetailsForDisplayAsync()
        {
            var currentPlan = await _scheduleRepository.GetCurrentPlanAsync();
            var nextPlan = await _scheduleRepository.GetNextPlanAsync();

            var startDate = currentPlan?.EffectiveDate ?? DateOnly.FromDateTime(DateTime.Today);
            var endDate = nextPlan?.EffectiveDate.AddDays(-1) ?? DateOnly.FromDateTime(DateTime.Today);

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

        

    }
}
