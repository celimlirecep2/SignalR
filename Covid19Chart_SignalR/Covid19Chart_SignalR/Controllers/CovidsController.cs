using Covid19Chart_SignalR.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Covid19Chart_SignalR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidsController : ControllerBase
    {
        private readonly CovidService _service;

        public CovidsController(CovidService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> SaveCovid(Covid covid)
        {
            await _service.SaveCovid(covid);
            //IQueryable covidList = _service.GetList();
            return Ok(_service.GetCovidChartList());
        }

        [HttpGet]
        public IActionResult InitializeCovid()
        {
            Random rnd = new Random();
            Enumerable.Range(1, 10).ToList().ForEach( x =>
            {
                foreach (ECity item in Enum.GetValues(typeof(ECity)))
                {
                    var newCovid = new Covid { City = item, Count = rnd.Next(100, 1000), CovidDate =DateTime.Now.AddDays(x) };
                    //await _service.SaveCovid(newCovid);iki türlüde kullanılabilir
                    _service.SaveCovid(newCovid).Wait();
                    //kaydetmelerde 1 er saniye beklesin
                    System.Threading.Thread.Sleep(1000);
                }
            });
            return Ok("Covid verileri veri tabanına kaydedildi");
        }
    }
}
