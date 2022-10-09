using Microsoft.AspNetCore.SignalR;

namespace SignalRApp.WEB.Hubs
{
    public class MyHubWeb:Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage",message);
        }
    }
}
