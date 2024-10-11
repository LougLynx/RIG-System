using System.Collections.Generic;
using System.Threading.Tasks;
using Manage_Receive_Issues_Goods.Models;

namespace Manage_Receive_Issues_Goods.Repository
{
    public interface ISchedulereceivedTLIPRepository
    {
        Task<IEnumerable<Schedulereceived>> GetAllAsync();
        Task<Schedulereceived> GetByIdAsync(int id);
        Task<IEnumerable<Schedulereceived>> GetSchedulesByWeekdayAsync(int weekdayId);
        Task AddAsync(Schedulereceived entity);
        Task UpdateAsync(Schedulereceived entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<Actualreceived>> GetAllActualReceivedAsync();
        Task<IEnumerable<Supplier>> GetSuppliersForTodayAsync(int weekdayId);
        Task<Schedulereceived> GetScheduleBySupplierIdAsync(int supplierId);
        Task<IEnumerable<AsnInformation>> GetAsnInformationAsync(DateTime inputDate);
        Task<IEnumerable<AsnDetailData>> GetAsnDetailAsync(string asnNumber, string doNumber, string invoice);
    }
}
