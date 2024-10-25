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

        /*public async Task UpdateCalendar(Actualreceivedtlip actualReceived)
        {
            _logger.LogInformation("UpdateCalendar called with DATA AHHAHAHAHA: {actualReceived}", actualReceived);
            await Clients.All.SendAsync("UpdateCalendar", actualReceived);
               
        }*/
    }
}
