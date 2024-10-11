using System.Collections.Generic;
using System.Threading.Tasks;
using Manage_Receive_Issues_Goods.DTO;
using Manage_Receive_Issues_Goods.Models;

namespace Manage_Receive_Issues_Goods.Repository
{
    public interface IScheduleRITDRepository
    {
        Task<IEnumerable<Status>> GetAllStatusesAsync();
        Task<IEnumerable<Planritddetail>> GetPlanDetails(int planId);
        Task<IEnumerable<Planritddetail>> GetAllPlanDetailsAsync();
        Task<Planritddetail> GetPlanDetailByIdAsync(int detailId);
        Task UpdatePlanDetailAsync(Planritddetail detail);
        Task AddActualAsync(Actualsritd actual);
        Task DeleteActualAsync(int actualId);
        Task AddPlanAsync(Planritd plan);
        Task AddPlanDetailAsync(Planritddetail planDetail);
        Task<int> GetPlanIdByDetailsAsync(string planName, string planType, DateOnly effectiveDate);
        Task<Planritd> GetCurrentPlanAsync();
        Task<Planritd> GetNextPlanAsync();
        Task<IEnumerable<Planritddetail>> GetPlanDetailsBetweenDatesAsync(DateOnly startDate, DateOnly endDate);
        Task<IEnumerable<Planritd>> GetFuturePlansAsync();
        Task<IEnumerable<Planritddetail>> GetPastPlanDetailsAsync();
    }
}
