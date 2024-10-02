using System.Collections.Generic;
using System.Threading.Tasks;
using Manage_Receive_Issues_Goods.Models;

namespace Manage_Receive_Issues_Goods.Services
{
    public interface ISchedulereceivedTLIPService
    {
        Task<IEnumerable<Schedulereceived>> GetAllSchedulesAsync();
        Task<Schedulereceived> GetScheduleByIdAsync(int id);
        Task<IEnumerable<Schedulereceived>> GetSchedulesByWeekdayAsync(int weekdayId);
        Task AddScheduleAsync(Schedulereceived schedule);
        Task UpdateScheduleAsync(Schedulereceived schedule);
        Task DeleteScheduleAsync(int id);
        Task<IEnumerable<Actualreceived>> GetAllActualReceivedAsync();
        DateTime GetDateForWeekday(int year, int weekOfYear, int weekdayId);
        int GetWeekOfYear(DateTime date);
        Task<IEnumerable<Supplier>> GetSuppliersForTodayAsync();
        Task<Schedulereceived> GetScheduleBySupplierIdAsync(int supplierId);
        Task<bool> DelaySupplierAsync(int supplierId);
    }
}
