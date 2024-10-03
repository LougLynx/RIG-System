using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<Planritddetail>> GetPlanDetailsForWeekAsync()
        {
            return await _scheduleRepository.GetPlanDetailsForWeekAsync();
        }

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
    }
}
