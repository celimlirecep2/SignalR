using Covid19Chart_SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Covid19Chart_SignalR.Models
{
    public class CovidService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<CovidHub> _hubContext;

        public CovidService(AppDbContext context, IHubContext<CovidHub> hubContext)
        {
            _hubContext = hubContext;
            _context = context;
        }

        public IQueryable<Covid> GetList()
        {
            return _context.Covids.AsQueryable();
        }
        public async Task SaveCovid(Covid covid)
        {
            await _context.Covids.AddAsync(covid);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveCovidList",GetCovidChartList());
        }

        public List<CovidChart> GetCovidChartList()
        {
            List<CovidChart> covidCharts = new List<CovidChart>();
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SELECT tarih,[1],[2],[3],[4],[5] FROM (SELECT[City],[Count], CAST([CovidDate] as date) as tarih FROM Covids) as covidT PIVOT (SUM(Count) FOR City IN([1],[2],[3],[4],[5])) as pTable Order By tarih asc";
                command.CommandType = System.Data.CommandType.Text;
                _context.Database.OpenConnection();
                using (var reader=command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CovidChart covidChart = new CovidChart();
                        covidChart.CovidDate = reader.GetDateTime(0).ToShortDateString();
                        Enumerable.Range(1, 5).ToList().ForEach(x =>
                        {
                            if (System.DBNull.Value.Equals(reader[x]))
                            {
                                covidChart.Counts.Add(0);
                            }
                            else
                            {
                                covidChart.Counts.Add(reader.GetInt32(x));
                            }
                        });
                        covidCharts.Add(covidChart);
                    }
                }
                _context.Database.CloseConnection();
                return covidCharts;
            }
        }
    }
}
