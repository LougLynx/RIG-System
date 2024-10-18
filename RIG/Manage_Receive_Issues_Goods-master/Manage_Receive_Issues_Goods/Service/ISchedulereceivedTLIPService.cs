using System.Collections.Generic;
using System.Threading.Tasks;
using Manage_Receive_Issues_Goods.Models;

namespace Manage_Receive_Issues_Goods.Services
{
    public interface ISchedulereceivedTLIPService
    {
        Task<IEnumerable<Plandetailreceivedtlip>> GetAllSchedulesAsync();
        Task<Plandetailreceivedtlip> GetScheduleByIdAsync(int id);
        Task<IEnumerable<Plandetailreceivedtlip>> GetSchedulesByWeekdayAsync(int weekdayId);
        Task AddScheduleAsync(Plandetailreceivedtlip schedule);
        Task UpdateScheduleAsync(Plandetailreceivedtlip schedule);
        Task DeleteScheduleAsync(int id);
        Task<IEnumerable<Actualreceivedtlip>> GetAllActualReceivedAsync();
        DateTime GetDateForWeekday(int year, int weekOfYear, int weekdayId);
        int GetWeekOfYear(DateTime date);
        Task<IEnumerable<Supplier>> GetSuppliersForTodayAsync();
        Task<Plandetailreceivedtlip> GetScheduleBySupplierIdAsync(string supplierId);
        Task<bool> DelaySupplierAsync(string supplierId);
        Task<IEnumerable<AsnInformation>> GetAsnInformationAsync(DateTime inputDate);
        Task<IEnumerable<AsnDetailData>> GetAsnDetailAsync(string asnNumber, string doNumber, string invoice);
        Task AddActualReceivedAsync(Actualreceivedtlip actualReceived);
        Task UpdateActualDetailTLIPAsync(string partNo, int actualReceivedId, int quantityRemain);
        Task<IEnumerable<Actualdetailtlip>> GetActualDetailsByReceivedIdAsync(int actualReceivedId);
    }
}
