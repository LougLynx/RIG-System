using System.Collections.Generic;
using System.Threading.Tasks;
using Manage_Receive_Issues_Goods.Models;

namespace Manage_Receive_Issues_Goods.Repository
{
    public interface ISchedulereceivedTLIPRepository
    {
        Task<IEnumerable<Planreceivetlip>> GetAllPlanAsync();
        Task<IEnumerable<Plandetailreceivedtlip>> GetAllPlanDetailAsync();
        Task<Plandetailreceivedtlip> GetPlanDetailByIdAsync(int id);
        Task<IEnumerable<Plandetailreceivedtlip>> GetSchedulesByWeekdayAsync(int weekdayId);
        //Task AddAsync(Plandetailreceivedtlip entity);
        //Task UpdateAsync(Plandetailreceivedtlip entity);
        //Task DeleteAsync(int id);
        Task<IEnumerable<Actualreceivedtlip>> GetAllActualReceivedAsync();
        Task<IEnumerable<Supplier>> GetSuppliersForTodayAsync(int weekdayId);
        Task<Plandetailreceivedtlip> GetScheduleBySupplierIdAsync(string supplierId);
        Task<IEnumerable<AsnInformation>> GetAsnInformationAsync(DateTime inputDate);
        Task<IEnumerable<AsnDetailData>> GetAsnDetailAsync(string asnNumber, string doNumber, string invoice);
        Task AddActualReceivedAsync(Actualreceivedtlip actualReceived);
        Task UpdateActualDetailTLIPAsync(string partNo, int actualReceivedId, int quantityRemain);
        Task<IEnumerable<Actualdetailtlip>> GetActualDetailsByReceivedIdAsync(int actualReceivedId);
		Task<Actualreceivedtlip> GetActualReceivedWithSupplierAsync(int actualReceivedId);
        Task<Actualreceivedtlip> GetActualReceivedEntryAsync(string supplierCode, DateTime actualDeliveryTime, string asnNumber);
        Task AddActualDetailAsync(Actualdetailtlip actualDetail);
        Task UpdateActualReceivedAsync(Actualreceivedtlip actualReceived);
        Task<IEnumerable<Actualreceivedtlip>> GetAllActualReceivedLast7DaysAsync();
        Task<IEnumerable<Actualreceivedtlip>> GetAllActualReceivedAsyncById(int actualReceivedId);
        Task<IEnumerable<Plandetailreceivedtlip>> GetAllPlanDetailByPlanIdAsync(int planId);
        Task<IEnumerable<Plandetailreceivedtlip>> GetAllCurrentPlanDetailsAsync();
    }
}
