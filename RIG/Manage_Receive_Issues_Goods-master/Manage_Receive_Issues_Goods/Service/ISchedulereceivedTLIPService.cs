using System.Collections.Generic;
using System.Threading.Tasks;
using Manage_Receive_Issues_Goods.DTO;
using Manage_Receive_Issues_Goods.Models;

namespace Manage_Receive_Issues_Goods.Services
{
    public interface ISchedulereceivedTLIPService
    {
        Task<IEnumerable<Actualreceivedtlip>> GetAllActualReceivedAsync();
        //DateTime GetDateForWeekday(int year, int weekOfYear, int weekdayId);
        DateTime GetDateForWeekday(int weekdayId);
        int GetWeekOfYear(DateTime date);
        Task<IEnumerable<Supplier>> GetSuppliersForTodayAsync();
        Task<bool> DelaySupplierAsync(string supplierId);//(pending)
        Task<IEnumerable<AsnInformation>> GetAsnInformationAsync(DateTime inputDate);
        Task<IEnumerable<AsnDetailData>> GetAsnDetailAsync(string asnNumber, string doNumber, string invoice);
        Task AddActualReceivedAsync(Actualreceivedtlip actualReceived);
        Task UpdateActualDetailTLIPAsync(string partNo, int actualReceivedId, int? quantityRemain, int? quantityScan);
        Task<IEnumerable<Actualdetailtlip>> GetActualDetailsByReceivedIdAsync(int actualReceivedId);
        Task<Actualreceivedtlip> GetActualReceivedWithSupplierAsync(int actualReceivedId);
        Task<Actualreceivedtlip> GetActualReceivedEntryAsync(string supplierCode, string actualDeliveryTime, string asnNumber = null, string doNumber = null, string invoice = null);
        Task AddActualDetailAsync(Actualdetailtlip actualDetail);
        Task UpdateActualReceivedAsync(Actualreceivedtlip actualReceived);
        Task<IEnumerable<Actualreceivedtlip>> GetAllActualReceivedLast7DaysAsync();
        Task<IEnumerable<Actualreceivedtlip>> GetAllActualReceivedAsyncById(int actualReceivedId);
        Task<IEnumerable<Plandetailreceivedtlip>> GetAllCurrentPlanDetailsAsync();
        Task<IEnumerable<Supplier>> GetSuppliersByWeekdayAsync(int weekdayId);
        Task<IEnumerable<Actualreceivedtlip>> GetActualReceivedAsyncByInfor(string asnNumber, string doNumber, string invoice);
        Task UpdateActualReceivedCompletionAsync(int actualReceivedId, bool isCompleted);
        Task<Actualreceivedtlip> GetActualReceivedByDetailsAsync(ActualReceivedTLIPDTO details);
        Task AddAllPlanDetailsToHistoryAsync();
        Task AddAllActualToHistoryAsync(int actualReceivedId);
        Task<IEnumerable<Historyplanreceivedtlip>> GetPlanActualDetailsInHistoryAsync();
        Task<IEnumerable<(Supplier Supplier, int TripCount)>> GetSuppliersWithTripCountForTodayAsync();
        Task<IEnumerable<TripCountTLIPDTO>> GeActualTripCountForTodayAsync();
        Task<IEnumerable<Actualreceivedtlip>> GetActualReceivedBySupplierForTodayAsync(string supplierCode);
        Task<IEnumerable<Plandetailreceivedtlip>> GetAllCurrentPlanDetailsBySupplierCodeAsync(string supplierCode);
        Task<IEnumerable<Actualreceivedtlip>> GetAsnDetailInDataBaseAsync(string asnNumber, string doNumber, string invoice);
        Task<IEnumerable<Supplier>> GetAllSuppliersAsync();
        Task UpdateActualLeadTime(Actualreceivedtlip actualReceived, DateTime leadTimeUpdate);
        Task<List<Actualreceivedtlip>> GetIncompleteActualReceived();
        ActualReceivedTLIPDTO MapToActualReceivedTLIPDTO(Actualreceivedtlip entity);
        PlanDetailTLIPDTO MapToPlanDetailTLIPDTO(Plandetailreceivedtlip entity);
        Task<IEnumerable<Tagnamereceivetlip>> GetAllTagNameRuleAsync();
    }
}
