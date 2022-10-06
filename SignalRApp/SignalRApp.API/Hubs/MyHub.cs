using Microsoft.AspNetCore.SignalR;

namespace SignalRApp.API.Hubs
{
    public class MyHub:Hub
    {
        private static List<string> Names { get; set; } = new List<string>();
        private static int ClientCount { get; set; } = 0;
        public async Task SendName(string name)
        {
            Names.Add(name);
            await   Clients.All.SendAsync("ReceiveName", name);//hub içerisindeki clientlerin hepsine mesajı gönderir ve onlarda bu mesajı okur
        }
        public async Task GetNames()
        {
            await Clients.All.SendAsync("ReceiveName", Names);
        }

        //herbir kullanıcı bağlandığında bu method çalışır 
        public async override Task OnConnectedAsync()
        {
            ClientCount++;
            await Clients.All.SendAsync("ReceiveClientCount", ClientCount);
            await base.OnConnectedAsync();
        }
        //herbir kullanıcı bağlandıyı kopardığında bu method çalışır 
        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            ClientCount--;
            await Clients.All.SendAsync("ReceiveClientCount", ClientCount);
            await base.OnConnectedAsync();
        }
    }
}
