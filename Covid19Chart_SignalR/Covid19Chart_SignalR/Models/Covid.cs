using System.ComponentModel.DataAnnotations;

namespace Covid19Chart_SignalR.Models
{
    public class Covid
    {
        public int Id { get; set; } 
        public ECity City { get; set; } 
        public int Count { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CovidDate { get; set; }

    }

    public enum ECity
    {
        Istanbul=1,
        Ankara=2,
        Izmır=3,
        Konya=4,
        Antalya=5
    }
}
