using Microsoft.AspNetCore.SignalR;

namespace SignalRApp.API.Hubs
{
    public class MyHub:Hub
    {
        public static List<string> Names { get; set; } = new List<string>();
        public async Task SendName(string name)
        {
            Names.Add(name);
            await   Clients.All.SendAsync("ReceiveName", name);//hub içerisindeki clientlerin hepsine mesajı gönderir ve onlarda bu mesajı okur
        }
        public async Task GetNames()
        {
            await Clients.All.SendAsync("ReceiveName", Names);
        }
    }
}
