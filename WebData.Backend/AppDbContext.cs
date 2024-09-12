using Microsoft.EntityFrameworkCore;
using WebData.Objects.PageContext.Objs;

namespace WebData.Backend
{
    /// <summary>
    /// Definiert die Datenbank-Infrastruktur des Entity-Frameworks
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        /// <summary>
        /// Definiert eine Datenbank-Sammlung an Benutzeraufgaben
        /// </summary>
        public DbSet<UserTasks> UserTasks { get; set; }

        /// <summary>
        /// Definiert eine Datenbank-Sammlung an Benutzern
        /// </summary>
        public DbSet<UserObject> Users { get; set; }

        /// <summary>
        /// Definiert eine Datenbank-Sammlung an Nachrichten
        /// </summary>
        public DbSet<NewsObject> News { get; set; }

        /// <summary>
        /// Definiert eine Datenbank-Sammlung n:m an Zugewießenen Aufgaben 
        /// </summary>
        /// <remarks>
        /// Welchem Benutzer welche Aufgaben zugewießen wurden
        /// </remarks>
        public DbSet<ZugewieseneAufgabe> ZugewieseneAufgaben { get; set; }

        /// <summary>
        /// Wenn das Model erstellt wird, werden hier die Tabellen-Verbindungen zugewießen
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ZugewieseneAufgabe>()
                .HasKey(ua => new { ua.UserId, ua.UserTaskId });

            modelBuilder.Entity<ZugewieseneAufgabe>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.UserAssignedTasks)
                .HasForeignKey(ua => ua.UserId);

            modelBuilder.Entity<ZugewieseneAufgabe>()
                .HasOne(ua => ua.UserTask)
                .WithMany(a => a.UserAssignedTasks)
                .HasForeignKey(ua => ua.UserTaskId);
        }
    }
}
