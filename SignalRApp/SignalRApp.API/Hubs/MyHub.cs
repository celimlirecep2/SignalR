using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SignalRApp.API.Models;

namespace SignalRApp.API.Hubs
{
    public class MyHub : Hub
    {
        private readonly AppDbContext _context;

        public MyHub(AppDbContext context)
        {
            _context = context;
        }

        private static List<string> Names { get; set; } = new List<string>();
        private static int ClientCount { get; set; } = 0;
        /// <summary>
        /// https://localhost:7221/api/Notification/8
        /// istekte bulunarak odaya yazılabilicek en fazla karakter sayısı ayarlanmış olur
        /// </summary>
        public static int teamCount { get; set; } = 7;
        public async Task SendName(string name)
        {
            if (Names.Count >= teamCount)
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

        //Groups
        public async Task AddToGroup(string teamName)
        {
            //huba her bağlanıldığında bir connectionId tanımlaması yapılır
            await Groups.AddToGroupAsync(Context.ConnectionId, teamName);
        }
        public async Task RemoveGroup(string TeamName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, TeamName);
        }
        //group a yazma
        public async Task SendNameByGroup(string name, string teamName)
        {
            var team = _context.Teams.Where(i => i.Name == teamName).FirstOrDefault();
            if (team != null)
            {
                team.Users.Add(new User { Name = name });
            }
            else
            {
                var newTeam = new Team { Name = teamName };
                newTeam.Users.Add(new User { Name = name });
                _context.Teams.Add(newTeam);
            }
            await _context.SaveChangesAsync();

            await Clients.Group(teamName).SendAsync("ReceiveMessageByGroup", name, team.Id);
        }
        //groupta daha öncesinde konuşulanlar varsa görme
        public async Task GetNamesByGroup()
        {
            //tüm grupların konuşmalarını getirdik
            var teams = _context.Teams.Include(i => i.Users).Select(x => new
            {
                teamId = x.Id,
                Users = x.Users.ToList()
            });
            //signalR çevirirken nesneyi sorun yaşadı çevirip aktardım
            //client taraftada string gittiğinden json parse işlemi uyguladım
            var stringTeams= JsonConvert.SerializeObject(teams, Formatting.Indented);
            await Clients.Caller.SendAsync("ReceiveNamesByGroup", stringTeams);
        }


        public async Task SendProduct(Product p)
        {
          await  Clients.All.SendAsync("ReceiveProduct",p);
        }
    }
}
