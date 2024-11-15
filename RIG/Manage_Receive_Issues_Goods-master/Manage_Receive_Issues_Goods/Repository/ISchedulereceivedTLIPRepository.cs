using System.Collections.Generic;
using System.Threading.Tasks;
using Manage_Receive_Issues_Goods.DTO;
using Manage_Receive_Issues_Goods.Models;

namespace Manage_Receive_Issues_Goods.Repository
{
    public interface ISchedulereceivedTLIPRepository
    {
        
        Task<IEnumerable<Actualreceivedtlip>> GetAllActualReceivedAsync();
        Task<IEnumerable<Supplier>> GetSuppliersForTodayAsync(int weekdayId);
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
        Task<IEnumerable<Actualreceivedtlip>> GetActualReceivedAsyncByInfor(string asnNumber, string doNumber, string invoice);
        Task UpdateActualReceivedCompletionAsync(int actualReceivedId, bool isCompleted);
        Task<Actualreceivedtlip> GetActualReceivedByDetailsAsync(ActualReceivedTLIPDTO details);
        Task AddHistoryPlanReceivedAsync(Historyplanreceivedtlip historyPlanReceived);
        Task<bool> ExistsInHistoryPlanReceivedAsync(int planDetailId, DateOnly date);
        Task<IEnumerable<Historyplanreceivedtlip>> GetPlanActualDetailsInHistoryAsync();
        Task<int> GetSupplierTripCountAsync(string supplierCode, int weekdayId);
        Task<IEnumerable<TripCountTLIPDTO>> GeActualTripCountForTodayAsync();
        Task<IEnumerable<Actualreceivedtlip>> GetActualReceivedBySupplierForTodayAsync(string supplierCode);
        Task<IEnumerable<Plandetailreceivedtlip>> GetAllCurrentPlanDetailsBySupplierCodeAsync(string supplierCode);
        Task<IEnumerable<Actualreceivedtlip>> GetAsnDetailInDataBaseAsync(string asnNumber, string doNumber, string invoice);
        Task<IEnumerable<Supplier>> GetAllSuppliersAsync();
        Task<IEnumerable<Tagnamereceivetlip>> GetAllTagNameRuleAsync();
    }
}
