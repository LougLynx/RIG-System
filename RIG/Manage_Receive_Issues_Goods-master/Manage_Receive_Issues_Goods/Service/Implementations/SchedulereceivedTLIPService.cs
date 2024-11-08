using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Manage_Receive_Issues_Goods.DTO;
using Manage_Receive_Issues_Goods.Models;
using Manage_Receive_Issues_Goods.Repositories;
using Manage_Receive_Issues_Goods.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Manage_Receive_Issues_Goods.Services
{
    public class SchedulereceivedTLIPService : ISchedulereceivedTLIPService
    {
        private readonly ISchedulereceivedTLIPRepository _repository;

        public SchedulereceivedTLIPService(ISchedulereceivedTLIPRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Actualreceivedtlip>> GetAllActualReceivedAsync()
        {
            return await _repository.GetAllActualReceivedAsync();
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersForTodayAsync()
        {
            // Lấy thứ hiện tại (0 = Sunday, 1 = Monday, ...)
            int currentWeekday = (int)DateTime.Now.DayOfWeek; 
            // Chuyển đổi 0 (Chủ Nhật) thành 7 cho phù hợp với bảng Weekday
            if (currentWeekday == 0) currentWeekday = 7; 

            var suppliers = await _repository.GetSuppliersForTodayAsync(currentWeekday);
            return suppliers;
        }
        public async Task<IEnumerable<Supplier>> GetSuppliersByWeekdayAsync(int weekdayId)
        {
            var suppliers = await _repository.GetSuppliersForTodayAsync(weekdayId);
            return suppliers;
        }
        public async Task<IEnumerable<TripCountTLIPDTO>> GeActualTripCountForTodayAsync()
        {
            return await _repository.GeActualTripCountForTodayAsync(); ;
        }

        public async Task<IEnumerable<(Supplier Supplier, int TripCount)>> GetSuppliersWithTripCountForTodayAsync()
        {
            int currentWeekday = (int)DateTime.Now.DayOfWeek;
            if (currentWeekday == 0) currentWeekday = 7;

            var suppliers = await _repository.GetSuppliersForTodayAsync(currentWeekday);
            var supplierTripCounts = new List<(Supplier Supplier, int TripCount)>();

            foreach (var supplier in suppliers)
            {
                var tripCount = await _repository.GetSupplierTripCountAsync(supplier.SupplierCode, currentWeekday);
                supplierTripCounts.Add((supplier, tripCount));
            }

            return supplierTripCounts;
        }

        public async Task<int> GetSupplierTripCountAsync(string supplierCode, int weekdayId)
        {
            return await _repository.GetSupplierTripCountAsync(supplierCode, weekdayId);
        }
        public async Task<IEnumerable<Plandetailreceivedtlip>> GetAllCurrentPlanDetailsAsync()
        {
            return await _repository.GetAllCurrentPlanDetailsAsync();
        }
        public async Task<IEnumerable<Plandetailreceivedtlip>> GetAllCurrentPlanDetailsBySupplierCodeAsync(string supplierCode)
        {
            return await _repository.GetAllCurrentPlanDetailsBySupplierCodeAsync(supplierCode);
        }
        // Hàm tính toán ngày giao hàng cụ thể cho một lịch trình nhận hàng
        public DateTime GetDateForWeekday(int year, int weekOfYear, int weekdayId)
        {
            DateTime jan1 = new DateTime(year, 1, 1); // Ngày đầu tiên của năm
            Calendar cal = CultureInfo.CurrentCulture.Calendar;

            // Tính toán số ngày cần thêm từ ngày đầu  tuần (thứ Hai)
            int daysOffset = DayOfWeek.Monday - jan1.DayOfWeek;
            DateTime firstMonday = jan1.AddDays(daysOffset);

            // Lấy số tuần đầu tiên
            int firstWeek = cal.GetWeekOfYear(jan1, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            // Tính toán ngày đầu tuần mong muốn (tuần weekOfYear)
            DateTime targetWeekStart = firstMonday.AddDays((weekOfYear - firstWeek) * 7);

            // Thứ của tuần từ `WeekdayID` (WeekdayID: 1 = Monday, 2 = Tuesday, ...)
            DateTime targetDate = targetWeekStart.AddDays(weekdayId - 1);

            return targetDate;
        }
        public DateTime GetDateForWeekday(int weekdayId)
        {
            DateTime today = DateTime.Today;
            int currentWeekday = (int)today.DayOfWeek;

            // Calculate the target date within the next 7 days
            int daysToAdd = (weekdayId - currentWeekday + 7) % 7;
            DateTime targetDate = today.AddDays(daysToAdd);

            return targetDate;
        }



        // Hàm tính tuần trong năm
        public int GetWeekOfYear(DateTime date)
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            return culture.Calendar.GetWeekOfYear(date, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }



        public async Task<bool> DelaySupplierAsync(string supplierId)
        {
            /* var schedule = await GetScheduleBySupplierIdAsync(supplierId);
             if (schedule == null) return false;

             var allSchedules = await GetSchedulesByWeekdayAsync(schedule.WeekdayId);
             var otherSchedules = allSchedules.Where(s => s.SupplierCode != supplierId).OrderBy(s => s.DeliveryTime.Time1).ToList();

             foreach (var otherSchedule in otherSchedules)
             {
                 if (otherSchedule.LeadTime <= schedule.LeadTime)
                 {
                     // Swap delivery times
                     var tempTime = schedule.DeliveryTime;
                     schedule.DeliveryTime = otherSchedule.DeliveryTime;
                     otherSchedule.DeliveryTime = tempTime;

                     await UpdateScheduleAsync(schedule);
                     await UpdateScheduleAsync(otherSchedule);
                     return true;
                 }
             }

             // If no suitable schedule found, move to end of the day
             schedule.DeliveryTime = new Time { Time1 = TimeOnly.FromDateTime(DateTime.Today.AddHours(23).AddMinutes(59)) };
             await UpdateScheduleAsync(schedule);*/
            return true;
        }

        public async Task<IEnumerable<AsnInformation>> GetAsnInformationAsync(DateTime inputDate)
        {
            return await _repository.GetAsnInformationAsync(inputDate);
        }

        public async Task<IEnumerable<AsnDetailData>> GetAsnDetailAsync(string asnNumber, string doNumber, string invoice)
        {
            return await _repository.GetAsnDetailAsync(asnNumber, doNumber, invoice);
        }

        public async Task AddActualReceivedAsync(Actualreceivedtlip actualReceived)
        {
            await _repository.AddActualReceivedAsync(actualReceived);
        }
        public async Task UpdateActualDetailTLIPAsync(string partNo, int actualReceivedId, int? quantityRemain, int? quantityScan)
        {
            await _repository.UpdateActualDetailTLIPAsync(partNo, actualReceivedId, quantityRemain, quantityScan);
        }
        public async Task<IEnumerable<Actualdetailtlip>> GetActualDetailsByReceivedIdAsync(int actualReceivedId)
        {
            return await _repository.GetActualDetailsByReceivedIdAsync(actualReceivedId);
        }
        public async Task<Actualreceivedtlip> GetActualReceivedWithSupplierAsync(int actualReceivedId)
        {
            return await _repository.GetActualReceivedWithSupplierAsync(actualReceivedId);
        }

        public async Task<Actualreceivedtlip> GetActualReceivedEntryAsync(string supplierCode, string actualDeliveryTime, string asnNumber = null, string doNumber = null, string invoice = null)
        {
            return await _repository.GetActualReceivedEntryAsync(supplierCode, actualDeliveryTime, asnNumber, doNumber, invoice);
        }

        public async Task AddActualDetailAsync(Actualdetailtlip actualDetail)
        {
            await _repository.AddActualDetailAsync(actualDetail);
        }

        public async Task UpdateActualReceivedAsync(Actualreceivedtlip actualReceived)
        {
            await _repository.UpdateActualReceivedAsync(actualReceived);
        }
        public async Task<IEnumerable<Actualreceivedtlip>> GetAllActualReceivedLast7DaysAsync()
        {
            return await _repository.GetAllActualReceivedLast7DaysAsync();
        }

        public async Task<IEnumerable<Actualreceivedtlip>> GetAllActualReceivedAsyncById(int actualReceivedId)
        {
            return await _repository.GetAllActualReceivedAsyncById(actualReceivedId);
        }
        public async Task<IEnumerable<Actualreceivedtlip>> GetActualReceivedAsyncByInfor(string asnNumber, string doNumber, string invoice)
        {
            return await _repository.GetActualReceivedAsyncByInfor(asnNumber, doNumber, invoice);
        }

        public async Task UpdateActualReceivedCompletionAsync(int actualReceivedId, bool isCompleted)
        {
            await _repository.UpdateActualReceivedCompletionAsync(actualReceivedId, isCompleted);
        }
        public async Task<Actualreceivedtlip> GetActualReceivedByDetailsAsync(ActualReceivedTLIPDTO details)
        {
            return await _repository.GetActualReceivedByDetailsAsync(details);
        }


        public async Task AddAllPlanDetailsToHistoryAsync()
        {
            var currentYear = DateTime.Now.Year;
            var currentWeekOfYear = GetWeekOfYear(DateTime.Now);
            var yesterday = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
            var planDetails = await _repository.GetAllCurrentPlanDetailsAsync();

            foreach (var planDetail in planDetails)
            {
                var exists = await _repository.ExistsInHistoryPlanReceivedAsync(planDetail.PlanDetailId, yesterday);
                if (!exists)
                {
                    var specificDate = GetDateForWeekday(currentYear, currentWeekOfYear,planDetail.WeekdayId);
                    if (specificDate.Date == yesterday.ToDateTime(TimeOnly.MinValue).Date)
                    {
                        var historyEntry = new Historyplanreceivedtlip
                        {
                            PlanDetailId = planDetail.PlanDetailId,
                            HistoryDate = yesterday
                        };
                        await _repository.AddHistoryPlanReceivedAsync(historyEntry);
                    }
                }
            }
        }



        public async Task AddAllActualToHistoryAsync(int actualReceivedId)
        {
            var historyEntry = new Historyplanreceivedtlip
            {
                ActualReceivedId = actualReceivedId,
                HistoryDate = DateOnly.FromDateTime(DateTime.Now)
            };
            await _repository.AddHistoryPlanReceivedAsync(historyEntry);
        }

        public async Task<IEnumerable<Historyplanreceivedtlip>> GetPlanActualDetailsInHistoryAsync()
        {
            return await _repository.GetPlanActualDetailsInHistoryAsync();
        }

        public async Task<IEnumerable<Actualreceivedtlip>> GetActualReceivedBySupplierForTodayAsync(string supplierCode)
        {
            return await _repository.GetActualReceivedBySupplierForTodayAsync(supplierCode);
        }
    }
}


