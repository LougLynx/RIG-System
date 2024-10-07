using System.Collections.Generic;
using System.Threading.Tasks;
using Manage_Receive_Issues_Goods.DTO;
using Manage_Receive_Issues_Goods.Models;

namespace Manage_Receive_Issues_Goods.Service
{
    public interface IScheduleRITDService
    {
        Task<IEnumerable<Planritd>> GetAllPlansAsync();
        Task<IEnumerable<Planritddetail>> GetAllPlanDetailsAsync();
        Task<IEnumerable<Actualsritd>> GetAllActualsAsync();
        Task<IEnumerable<Status>> GetAllStatusesAsync();
        /*Task<IEnumerable<Planritddetail>> GetPlanDetailsForWeekAsync();*/
        Task<Planritddetail> GetPlanDetailByIdAsync(int detailId);
        Task UpdatePlanDetailAsync(Planritddetail detail);
        Task<IEnumerable<Planritddetail>> GetAllPlanDetailsWithoutDateAsync();
        Task AddActualAsync(Actualsritd actual);
        Task DeleteActualAsync(int actualId);
        Task AddPlanAsync(Planritd plan);
        Task AddPlanDetailAsync(Planritddetail planDetail);
        Task<int> GetPlanIdByDetailsAsync(string planName, string planType, DateOnly effectiveDate);
        Task DeleteOldActualsAsync();
        Task<IEnumerable<PlanDetailDTO>> GetPlanAndActualDetailsAsync();
        Task<IEnumerable<PlanDetailDTO>> GetPlanDetailsForDisplayAsync();
    }
}
