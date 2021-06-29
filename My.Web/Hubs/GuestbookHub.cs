using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using My.Core.Models;

namespace My.Web.Hubs
{
    public class GuestbookHub : Hub
    {
        public async Task Send(GuestbookContent content)
        {
            await Clients.All.SendAsync("Updated", content);
        }
    }
}