using Microsoft.EntityFrameworkCore;
using Manage_Receive_Issues_Goods.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manage_Receive_Issues_Goods.Repository;
using System.Text.Json;
using Manage_Receive_Issues_Goods.Data;

namespace Manage_Receive_Issues_Goods.Repositories.Implementations
{
    public class SchedulereceivedTLIPRepository : ISchedulereceivedTLIPRepository
    {
        private readonly RigContext _context;
        private readonly HttpClient _httpClient;
        private readonly string _baseApiUrl = "http://10.73.131.20:8092/api/GetDataInfor/";

        public SchedulereceivedTLIPRepository(RigContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Schedulereceived>> GetAllAsync()
        {
            return await _context.Schedulereceiveds
                .Include(s => s.Supplier)
                .Include(s => s.DeliveryTime)
                .Include(s => s.Weekday)
                .ToListAsync();
        }

        public async Task<Schedulereceived> GetByIdAsync(int id)
        {
            return await _context.Schedulereceiveds
                .Include(s => s.Supplier)
                .Include(s => s.DeliveryTime)
                .Include(s => s.Weekday)
                .FirstOrDefaultAsync(s => s.ScheduleId == id);
        }

        public async Task<IEnumerable<Schedulereceived>> GetSchedulesByWeekdayAsync(int weekdayId)
        {
            return await _context.Schedulereceiveds
                .Include(s => s.Supplier)
                .Include(s => s.DeliveryTime)
                .Include(s => s.Weekday)
                .Where(s => s.WeekdayId == weekdayId)
                .ToListAsync();
        }

        public async Task AddAsync(Schedulereceived entity)
        {
            await _context.Schedulereceiveds.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Schedulereceived entity)
        {
            _context.Schedulereceiveds.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Schedulereceiveds.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Actualreceived>> GetAllActualReceivedAsync()
        {
            return await _context.Actualreceiveds
                .Include(ar => ar.Schedule) // Bao gồm thông tin lịch trình liên quan
                .ThenInclude(s => s.Supplier) // Bao gồm cả nhà cung cấp
                .ToListAsync();
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersForTodayAsync(int weekdayId)
        {
            return await _context.Schedulereceiveds
                .Where(s => s.WeekdayId == weekdayId)
                .OrderBy(s => s.DeliveryTime.Time1) // Sắp xếp theo giờ giao hàng
                .Select(s => s.Supplier)
                .Distinct()
                .ToListAsync();
        }

        public async Task<Schedulereceived> GetScheduleBySupplierIdAsync(int supplierId)
        {
            return await _context.Schedulereceiveds.FirstOrDefaultAsync(s => s.SupplierId == supplierId);
        }

        public async Task<IEnumerable<AsnInformation>> GetAsnInformationAsync(DateTime inputDate)
        {
            var response = await _httpClient.GetAsync($"{_baseApiUrl}GetAsnInformation?inputDate={inputDate:yyyy-MM-dd}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var jsonDocument = JsonDocument.Parse(content);
            var dataObjects = jsonDocument.RootElement.GetProperty("data").GetProperty("result");
            if (dataObjects.ValueKind != JsonValueKind.Null)
            {
                return dataObjects.EnumerateArray().Select(r => new AsnInformation
                {
                    AsnNumber = r.GetProperty("asnNumber").GetString(),
                    DoNumber = r.GetProperty("doNumber").GetString(),
                    Invoice = r.GetProperty("invoice").GetString(),
                    SupplierCode = r.GetProperty("supplierCode").GetString(),
                    SupplierName = r.GetProperty("supplierName").GetString(),
                    EtaDate = r.GetProperty("etaDate").GetDateTime(),
                    EtaDateString = r.GetProperty("etaDateString").GetString(),
                    ReceiveStatus = r.GetProperty("receiveStatus").GetBoolean(),
                    IsCompleted = r.GetProperty("isCompleted").GetBoolean()
                }).ToList();
            }
            return new List<AsnInformation>();
        }

        public async Task<IEnumerable<AsnDetailData>> GetAsnDetailAsync(string asnNumber, string doNumber, string invoice)
        {
            string queryParam = !string.IsNullOrEmpty(asnNumber) ? $"inputAsn={asnNumber}" :
                                !string.IsNullOrEmpty(doNumber) ? $"inputAsn={doNumber}" :
                                !string.IsNullOrEmpty(invoice) ? $"inputAsn={invoice}" : null;

            if (queryParam == null)
            {
                throw new ArgumentException("No valid parameter provided!");
            }

            var response = await _httpClient.GetAsync($"{_baseApiUrl}GetDetailDataByAsn?{queryParam}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var jsonDocument = JsonDocument.Parse(content);
            var dataObjects = jsonDocument.RootElement.GetProperty("data").GetProperty("result");
            if (dataObjects.ValueKind != JsonValueKind.Null)
            {
                return dataObjects.EnumerateArray().Select(r => new AsnDetailData
                {
                    PartNo = r.GetProperty("partNo").GetString(),
                    AsnNumber = r.GetProperty("asnNumber").GetString(),
                    DoNumber = r.GetProperty("doNumber").GetString(),
                    Invoice = r.GetProperty("invoice").GetString(),
                    Quantity = r.GetProperty("quantiy").GetInt32(),
                    QuantityRemain = r.GetProperty("quantityRemain").GetInt32()
                }).ToList();
            }
            return new List<AsnDetailData>();
        }

    }
}
