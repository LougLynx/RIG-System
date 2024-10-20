using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Manage_Receive_Issues_Goods.Models;
using Manage_Receive_Issues_Goods.Repositories;
using Manage_Receive_Issues_Goods.Repository;
using Microsoft.EntityFrameworkCore;

namespace Manage_Receive_Issues_Goods.Services
{
    public class SchedulereceivedTLIPService : ISchedulereceivedTLIPService
    {
        private readonly ISchedulereceivedTLIPRepository _repository;

        public SchedulereceivedTLIPService(ISchedulereceivedTLIPRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Plandetailreceivedtlip>> GetAllSchedulesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Plandetailreceivedtlip> GetScheduleByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Plandetailreceivedtlip>> GetSchedulesByWeekdayAsync(int weekdayId)
        {
            return await _repository.GetSchedulesByWeekdayAsync(weekdayId);
        }

        public async Task AddScheduleAsync(Plandetailreceivedtlip schedule)
        {
            await _repository.AddAsync(schedule);
        }

        public async Task UpdateScheduleAsync(Plandetailreceivedtlip schedule)
        {
            await _repository.UpdateAsync(schedule);
        }

        public async Task DeleteScheduleAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
        public async Task<IEnumerable<Actualreceivedtlip>> GetAllActualReceivedAsync()
        {
            return await _repository.GetAllActualReceivedAsync();
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersForTodayAsync()
        {
            int currentWeekday = (int)DateTime.Now.DayOfWeek; // Lấy thứ hiện tại (0 = Sunday, 1 = Monday, ...)
            if (currentWeekday == 0) currentWeekday = 7; // Chuyển đổi 0 (Chủ Nhật) thành 7 cho phù hợp với bảng Weekday

            var suppliers = await _repository.GetSuppliersForTodayAsync(currentWeekday);
            return suppliers;
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
        public async Task<Plandetailreceivedtlip> GetScheduleBySupplierIdAsync(string supplierId)
        {
            return await _repository.GetScheduleBySupplierIdAsync(supplierId);
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
        public async Task UpdateActualDetailTLIPAsync(string partNo, int actualReceivedId, int quantityRemain)
        {
            await _repository.UpdateActualDetailTLIPAsync(partNo, actualReceivedId, quantityRemain);
        }
        public async Task<IEnumerable<Actualdetailtlip>> GetActualDetailsByReceivedIdAsync(int actualReceivedId)
        {
            return await _repository.GetActualDetailsByReceivedIdAsync(actualReceivedId);
        }
		public async Task<Actualreceivedtlip> GetActualReceivedWithSupplierAsync(int actualReceivedId)
		{
			return await _repository.GetActualReceivedWithSupplierAsync(actualReceivedId);
		}

		public async Task<Actualreceivedtlip> GetActualReceivedEntryAsync(string supplierCode, DateTime actualDeliveryTime)
		{
			return await _repository.GetActualReceivedEntryAsync(supplierCode, actualDeliveryTime);
		}

		public async Task AddActualDetailAsync(Actualdetailtlip actualDetail)
		{
			await _repository.AddActualDetailAsync(actualDetail);
		}

        public async Task UpdateActualReceivedAsync(Actualreceivedtlip actualReceived)
        {
            await _repository.UpdateActualReceivedAsync(actualReceived);
        }
    }
    
}
