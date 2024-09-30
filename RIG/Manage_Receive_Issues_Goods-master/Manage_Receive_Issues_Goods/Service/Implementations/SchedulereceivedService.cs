using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Manage_Receive_Issues_Goods.Models;
using Manage_Receive_Issues_Goods.Repositories;
using Manage_Receive_Issues_Goods.Repository;

namespace Manage_Receive_Issues_Goods.Services
{
    public class SchedulereceivedService : ISchedulereceivedService
    {
        private readonly ISchedulereceivedRepository _repository;

        public SchedulereceivedService(ISchedulereceivedRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Schedulereceived>> GetAllSchedulesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Schedulereceived> GetScheduleByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Schedulereceived>> GetSchedulesByWeekdayAsync(int weekdayId)
        {
            return await _repository.GetSchedulesByWeekdayAsync(weekdayId);
        }

        public async Task AddScheduleAsync(Schedulereceived schedule)
        {
            await _repository.AddAsync(schedule);
        }

        public async Task UpdateScheduleAsync(Schedulereceived schedule)
        {
            await _repository.UpdateAsync(schedule);
        }

        public async Task DeleteScheduleAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
        public async Task<IEnumerable<Actualreceived>> GetAllActualReceivedAsync()
        {
            return await _repository.GetAllActualReceivedAsync();
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


    }
}
