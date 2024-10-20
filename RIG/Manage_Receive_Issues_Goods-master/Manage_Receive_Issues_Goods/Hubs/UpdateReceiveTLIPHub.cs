using Manage_Receive_Issues_Goods.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Manage_Receive_Issues_Goods.Hubs
{
    public class UpdateReceiveTLIPHub : Hub
    {
        private readonly ILogger<UpdateReceiveTLIPHub> _logger;
        private readonly RigContext _context;

        public UpdateReceiveTLIPHub(ILogger<UpdateReceiveTLIPHub> logger, RigContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task UpdateCalendar(Actualreceivedtlip actualReceived)
        {
            var actualReceivedEntry = await _context.Actualreceivedtlips
            .Include(a => a.SupplierCodeNavigation)
            .Include(a => a.Actualdetailtlips)  // Bao gồm luôn chi tiết để tính toán
            .Where(a => a.SupplierCode == actualReceived.SupplierCode && a.ActualDeliveryTime == actualReceived.ActualDeliveryTime)
            .OrderByDescending(a => a.ActualReceivedId)
            .FirstOrDefaultAsync();

            if (actualReceivedEntry != null)
            {

                var completionPercentage = CalculateCompletionPercentage(actualReceivedEntry);
                _logger.LogInformation("UpdateCalendar called with data: {@actualReceivedEntry}", actualReceivedEntry);
                await Clients.All.SendAsync("UpdateCalendar", new
                {
                    actualReceivedEntry.ActualDeliveryTime,
                    actualReceivedEntry.ActualLeadTime,
                    actualReceivedEntry.SupplierCodeNavigation.SupplierName,
                    actualReceived.AsnNumber,
                    actualReceived.DoNumber,
                    actualReceived.Invoice,
                    CompletionPercentage = completionPercentage
                });
            }
            else
            {
                _logger.LogWarning("No matching entry found for SupplierCode: {SupplierCode}, ActualDeliveryTime: {ActualDeliveryTime}", actualReceived.SupplierCode, actualReceived.ActualDeliveryTime);
            }
        }

        private double CalculateCompletionPercentage(Actualreceivedtlip actualReceived)
        {
            // Lấy tổng số lượng chi tiết trong ActualDetails
            var totalItems = actualReceived.Actualdetailtlips.Count;

            // Đếm số lượng chi tiết đã hoàn thành (QuantityRemain == 0)
            var completedItems = actualReceived.Actualdetailtlips.Count(detail => detail.QuantityRemain == 0);

            // Tính phần trăm hoàn thành
            if (totalItems == 0) return 0; // Tránh chia cho 0
            return (completedItems / (double)totalItems) * 100;
        }


    }

}
