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
                .Where(a => a.SupplierCode == actualReceived.SupplierCode && a.ActualDeliveryTime == actualReceived.ActualDeliveryTime)
                .OrderByDescending(a => a.ActualReceivedId)
                .FirstOrDefaultAsync();

            if (actualReceivedEntry != null)
            {
                _logger.LogInformation("UpdateCalendar called with data: {@actualReceivedEntry}", actualReceivedEntry);
                await Clients.All.SendAsync("UpdateCalendar", new
                {
                    actualReceivedEntry.ActualDeliveryTime,
                    actualReceivedEntry.ActualLeadTime,
                    SupplierName = actualReceivedEntry.SupplierCodeNavigation.SupplierName, // Ensure this matches the expected property name
                    actualReceived.AsnNumber,
                    actualReceived.DoNumber,
                    actualReceived.Invoice
                });
            }
            else
            {
                _logger.LogWarning("No matching entry found for SupplierCode: {SupplierCode}, ActualDeliveryTime: {ActualDeliveryTime}", actualReceived.SupplierCode, actualReceived.ActualDeliveryTime);
            }
        }

    }

}
