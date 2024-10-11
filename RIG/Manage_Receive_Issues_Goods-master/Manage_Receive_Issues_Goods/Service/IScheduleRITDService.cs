using System.Collections.Generic;
using System.Threading.Tasks;
using Manage_Receive_Issues_Goods.DTO;
using Manage_Receive_Issues_Goods.Models;

namespace Manage_Receive_Issues_Goods.Service
{
    public interface IScheduleRITDService
    {
        Task<IEnumerable<Planritddetail>> GetAllPlanDetailsAsync();
        Task<IEnumerable<Status>> GetAllStatusesAsync();
        Task<Planritddetail> GetPlanDetailByIdAsync(int detailId);
        Task UpdatePlanDetailAsync(Planritddetail detail);
        Task AddActualAsync(Actualsritd actual);
        Task DeleteActualAsync(int actualId);
        Task AddPlanAsync(Planritd plan);
        Task AddPlanDetailAsync(Planritddetail planDetail);
        Task<int> GetPlanIdByDetailsAsync(string planName, string planType, DateOnly effectiveDate);
        Task<IEnumerable<PlanDetailDTO>> GetPlanDetailsForDisplayAsync();
        Task<(Planritd currentPlan, Planritd nextPlan)> GetCurrentAndNextPlanAsync();
        Task<IEnumerable<PlanDTO>> GetFuturePlansAsync();
        Task<IEnumerable<Planritddetail>> GetPlanDetails(int planId);
        Task<IEnumerable<PlanDetailDTO>> GetPastPlanDetailsAsync();
    }
}
