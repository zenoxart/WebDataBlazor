using Microsoft.EntityFrameworkCore;
using WebData.Objects.PageContext.Objs;

namespace WebData.Backend
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Define your DbSets (tables) here
        public DbSet<Aufgabe> PersonenAufgaben { get; set; }
    }
}
