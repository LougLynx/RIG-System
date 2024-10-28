using Microsoft.EntityFrameworkCore;
using Manage_Receive_Issues_Goods.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manage_Receive_Issues_Goods.Repository;
using System.Text.Json;
using Manage_Receive_Issues_Goods.Data;
using Manage_Receive_Issues_Goods.DTO;

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

        public async Task<IEnumerable<Planreceivetlip>> GetAllPlanAsync()
        {
            return await _context.Planreceivetlips.ToListAsync();
        }

        public async Task<IEnumerable<Plandetailreceivedtlip>> GetAllPlanDetailAsync()
        {
            return await _context.Plandetailreceivedtlips
                .Include(s => s.SupplierCodeNavigation)
                .Include(s => s.Weekday)
                .Include(s => s.PlanId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Plandetailreceivedtlip>> GetAllPlanDetailByPlanIdAsync(int planId)
        {
            return await _context.Plandetailreceivedtlips
                .Include(s => s.SupplierCodeNavigation)
                .Include(s => s.Weekday)
                .Include(s => s.PlanId)
                .Where(s => s.PlanId == planId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Plandetailreceivedtlip>> GetAllCurrentPlanDetailsAsync()
        {
            // Lấy ra danh sách tất cả các kế hoạch
            var plans = await _context.Planreceivetlips
                .OrderBy(p => p.EffectiveDate)
                .ToListAsync();

            if (!plans.Any())
            {
                return Enumerable.Empty<Plandetailreceivedtlip>();
            }

            var today = DateOnly.FromDateTime(DateTime.Now);

            // Tìm kế hoạch gần nhất
            var closestPlan = plans
                .Where(p => p.EffectiveDate <= today)
                .OrderByDescending(p => p.EffectiveDate)
                .FirstOrDefault();

            if (closestPlan != null)
            {
                // Trả về danh sách các chi tiết kế hoạch của kế hoạch gần nhất
                return await _context.Plandetailreceivedtlips
                    .AsNoTracking()
                    .Include(s => s.SupplierCodeNavigation)
                    .Include(s => s.Weekday)
                    .Where(pd => pd.PlanId == closestPlan.PlanId)
                    .ToListAsync();
            }

            return Enumerable.Empty<Plandetailreceivedtlip>();
        }


        public async Task<Plandetailreceivedtlip> GetPlanDetailByIdAsync(int id)
        {
            return await _context.Plandetailreceivedtlips
                .Include(s => s.SupplierCodeNavigation)
                .Include(s => s.Weekday)
                .Include(s => s.PlanId)
                .FirstOrDefaultAsync(s => s.PlanDetailId == id);
        }

        public async Task<IEnumerable<Plandetailreceivedtlip>> GetSchedulesByWeekdayAsync(int weekdayId)
        {
            return await _context.Plandetailreceivedtlips
                .Include(s => s.SupplierCodeNavigation)
                .Include(s => s.Weekday)
                .Include(s => s.PlanId)
                .Where(s => s.WeekdayId == weekdayId)
                .ToListAsync();
        }

        /*        public async Task AddAsync(Plandetailreceivedtlip entity)
                {
                    await _context.Plandetailreceivedtlips.AddAsync(entity);
                    await _context.SaveChangesAsync();
                }*/

        /* public async Task UpdateAsync(Plandetailreceivedtlip entity)
         {
             _context.Plandetailreceivedtlips.Update(entity);
             await _context.SaveChangesAsync();
         }*/

        /*   public async Task DeleteAsync(int id)
           {
               var entity = await GetByIdAsync(id);
               if (entity != null)
               {
                   _context.Plandetailreceivedtlips.Remove(entity);
                   await _context.SaveChangesAsync();
               }
           }*/

        public async Task<IEnumerable<Actualreceivedtlip>> GetAllActualReceivedAsync()
        {
            return await _context.Actualreceivedtlips
                                 .Include(ar => ar.SupplierCodeNavigation)
                                 .Include(ar => ar.Actualdetailtlips)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Actualreceivedtlip>> GetAllActualReceivedAsyncById(int actualReceivedId)
        {
            return await _context.Actualreceivedtlips
                         .Include(ar => ar.SupplierCodeNavigation)
                         .Include(ar => ar.Actualdetailtlips)
                         .Where(ar => ar.ActualReceivedId == actualReceivedId)
                         .ToListAsync();
        }

        public async Task<IEnumerable<Actualreceivedtlip>> GetAllActualReceivedLast7DaysAsync()
        {
            var sevenDaysAgo = DateTime.Now.AddDays(-7);
            return await _context.Actualreceivedtlips
                                 .Include(ar => ar.SupplierCodeNavigation)
                                 .Include(ar => ar.Actualdetailtlips)
                                 .Where(ar => ar.ActualDeliveryTime >= sevenDaysAgo)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Actualreceivedtlip>> GetActualReceivedAsyncByInfor(string asnNumber, string doNumber, string invoice)
        {
            var query = _context.Actualreceivedtlips
                                .Include(ar => ar.SupplierCodeNavigation)
                                .Include(ar => ar.Actualdetailtlips)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(asnNumber))
            {
                query = query.Where(ar => ar.AsnNumber == asnNumber);
            }

            if (!string.IsNullOrEmpty(doNumber))
            {
                query = query.Where(ar => ar.DoNumber == doNumber);
            }

            if (!string.IsNullOrEmpty(invoice))
            {
                query = query.Where(ar => ar.Invoice == invoice);
            }

            return await query.ToListAsync();
        }



        public async Task<IEnumerable<Supplier>> GetSuppliersForTodayAsync(int weekdayId)
        {
            return await _context.Plandetailreceivedtlips
                .Where(s => s.WeekdayId == weekdayId)
                .OrderBy(s => s.DeliveryTime)
                .GroupBy(s => s.SupplierCode)
                .Select(g => g.First().SupplierCodeNavigation)
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
            string queryParam = null;

            if (!string.IsNullOrEmpty(asnNumber))
            {
                queryParam = $"inputAsn={asnNumber}";
            }
            else if (!string.IsNullOrEmpty(doNumber))
            {
                queryParam = $"inputAsn={doNumber}";
            }
            else if (!string.IsNullOrEmpty(invoice))
            {
                queryParam = $"inputAsn={invoice}";
            }


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

        public async Task<Actualreceivedtlip> GetActualReceivedEntryAsync(string supplierCode, DateTime actualDeliveryTime, string asnNumber)
        {
            return await _context.Actualreceivedtlips
                .Include(a => a.SupplierCodeNavigation)
                .Include(a => a.Actualdetailtlips)
                .Where(a => a.SupplierCode == supplierCode &&
                            a.ActualDeliveryTime == actualDeliveryTime &&
                            (string.IsNullOrEmpty(asnNumber) || a.AsnNumber == asnNumber))
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

        public async Task UpdateActualReceivedCompletionAsync(int actualReceivedId, bool isCompleted)
        {
            var actualReceived = await _context.Actualreceivedtlips.FindAsync(actualReceivedId);
            if (actualReceived == null)
            {
                throw new KeyNotFoundException("ActualReceived not found.");
            }

            actualReceived.IsCompleted = isCompleted;
            await _context.SaveChangesAsync();
        }

        public async Task<Actualreceivedtlip> GetActualReceivedByDetailsAsync(ActualReceivedTLIPDTO details)
        {
            return await _context.Actualreceivedtlips
                .FirstOrDefaultAsync(ar =>
                    ar.SupplierCode == details.SupplierCode &&
                    (string.IsNullOrEmpty(details.AsnNumber) || ar.AsnNumber == details.AsnNumber) &&
                    (string.IsNullOrEmpty(details.DoNumber) || ar.DoNumber == details.DoNumber) &&
                    (string.IsNullOrEmpty(details.Invoice) || ar.Invoice == details.Invoice));
        }
    }
}
