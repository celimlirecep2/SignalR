using Microsoft.AspNetCore.SignalR;

namespace SignalRApp.API.Hubs
{
    public class MyHub:Hub
    {
        private static List<string> Names { get; set; } = new List<string>();
        private static int ClientCount { get; set; } = 0;
        /// <summary>
        /// https://localhost:7221/api/Notification/8
        /// istekte bulunarak odaya yazılabilicek en fazla karakter sayısı ayarlanmış olur
        /// </summary>
        public static int teamCount { get; set; } = 7;
        public async Task SendName(string name)
        {
            if (Names.Count>=teamCount)
            {
                //sadece istek yapan clienta mesaj göndermek için kullanılır(caller)
                await Clients.Caller.SendAsync("Error", $"Takım en fazla {teamCount} kişi olabilir.");
            }
            else
            {

                Names.Add(name);
                await Clients.All.SendAsync("ReceiveName", name);//hub içerisindeki clientlerin hepsine mesajı gönderir ve onlarda bu mesajı okur
            }

        }
        public async Task GetNames()
        {
            await Clients.All.SendAsync("ReceiveNames", Names);
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
