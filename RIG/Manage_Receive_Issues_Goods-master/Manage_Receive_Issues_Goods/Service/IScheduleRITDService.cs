using System.Collections.Generic;
using System.Threading.Tasks;
using Manage_Receive_Issues_Goods.Models;

namespace Manage_Receive_Issues_Goods.Service
{
    public interface IScheduleRITDService
    {
        Task<IEnumerable<Planritd>> GetAllPlansAsync();
        Task<IEnumerable<Planritddetail>> GetAllPlanDetailsAsync();
        Task<IEnumerable<Actualsritd>> GetAllActualsAsync();
        Task<IEnumerable<Status>> GetAllStatusesAsync();
        Task<IEnumerable<Planritddetail>> GetPlanDetailsForWeekAsync();
        Task<Planritddetail> GetPlanDetailByIdAsync(int detailId);
        Task UpdatePlanDetailAsync(Planritddetail detail);
        Task<IEnumerable<Planritddetail>> GetAllPlanDetailsWithoutDateAsync();
    }
}
