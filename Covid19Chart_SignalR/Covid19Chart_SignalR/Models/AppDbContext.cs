using Microsoft.EntityFrameworkCore;

namespace Covid19Chart_SignalR.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        public DbSet<Covid> Covids { get; set; }

    }
}
