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

        public async Task<IEnumerable<Plandetailreceivedtlip>> GetAllAsync()
        {
            return await _context.Plandetailreceivedtlips
                .Include(s => s.SupplierCode)
                .Include(s => s.DeliveryTime)
                .Include(s => s.Weekday)
                .ToListAsync();
        }

        public async Task<Plandetailreceivedtlip> GetByIdAsync(int id)
        {
            return await _context.Plandetailreceivedtlips
                .Include(s => s.SupplierCode)
                .Include(s => s.DeliveryTime)
                .Include(s => s.Weekday)
                .FirstOrDefaultAsync(s => s.PlanDetailId == id);
        }

        public async Task<IEnumerable<Plandetailreceivedtlip>> GetSchedulesByWeekdayAsync(int weekdayId)
        {
            return await _context.Plandetailreceivedtlips
                .Include(s => s.SupplierCode)
                .Include(s => s.DeliveryTime)
                .Include(s => s.Weekday)
                .Where(s => s.WeekdayId == weekdayId)
                .ToListAsync();
        }

        public async Task AddAsync(Plandetailreceivedtlip entity)
        {
            await _context.Plandetailreceivedtlips.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Plandetailreceivedtlip entity)
        {
            _context.Plandetailreceivedtlips.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Plandetailreceivedtlips.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Actualreceivedtlip>> GetAllActualReceivedAsync()
        {
            return await _context.Actualreceivedtlips
                                 .Include(ar => ar.SupplierCodeNavigation) 
                                 .Include(ar => ar.Actualdetailtlips) 
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersForTodayAsync(int weekdayId)
        {
            return (IEnumerable<Supplier>)await _context.Plandetailreceivedtlips
                .Where(s => s.WeekdayId == weekdayId)
                .OrderBy(s => s.DeliveryTime) 
                .Select(s => s.SupplierCode)
                .Distinct()
                .ToListAsync();
        }

        public async Task<Plandetailreceivedtlip> GetScheduleBySupplierIdAsync(string supplierCode)
        {
            return await _context.Plandetailreceivedtlips.FirstOrDefaultAsync(s => s.SupplierCode == supplierCode);
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

        public async Task AddActualReceivedAsync(Actualreceivedtlip actualReceived)
        {
            _context.Actualreceivedtlips.Add(actualReceived);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateActualDetailTLIPAsync(string partNo, int actualReceivedId, int quantityRemain)
        {
            var actualDetail = await _context.Actualdetailtlips
                .FirstOrDefaultAsync(ad => ad.PartNo == partNo && ad.ActualReceivedId == actualReceivedId);

            if (actualDetail != null)
            {
                actualDetail.QuantityRemain = quantityRemain;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Actualdetailtlip>> GetActualDetailsByReceivedIdAsync(int actualReceivedId)
        {
            return await _context.Actualdetailtlips
                                 .Where(ad => ad.ActualReceivedId == actualReceivedId)
                                 .ToListAsync();
        }

		public async Task<Actualreceivedtlip> GetActualReceivedWithSupplierAsync(int actualReceivedId)
		{
			return await _context.Actualreceivedtlips
				.Include(a => a.SupplierCodeNavigation)
				.FirstOrDefaultAsync(a => a.ActualReceivedId == actualReceivedId);
		}

		public async Task<Actualreceivedtlip> GetActualReceivedEntryAsync(string supplierCode, DateTime actualDeliveryTime)
		{
			return await _context.Actualreceivedtlips
				.Include(a => a.SupplierCodeNavigation)
				.Include(a => a.Actualdetailtlips)
				.Where(a => a.SupplierCode == supplierCode && a.ActualDeliveryTime == actualDeliveryTime)
				.OrderByDescending(a => a.ActualReceivedId)
				.FirstOrDefaultAsync();
		}

		public async Task AddActualDetailAsync(Actualdetailtlip actualDetail)
		{
			_context.Actualdetailtlips.Add(actualDetail);
			await _context.SaveChangesAsync();
		}

        public async Task UpdateActualReceivedAsync(Actualreceivedtlip actualReceived)
        {
            _context.Actualreceivedtlips.Update(actualReceived);
            await _context.SaveChangesAsync();
        }

    }
}
