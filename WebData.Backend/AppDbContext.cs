using Microsoft.EntityFrameworkCore;
using WebData.Objects.PageContext.Objs;

namespace WebData.Backend
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Define your DbSets (tables) here
        public DbSet<UserTasks> UserTasks { get; set; }
        public DbSet<UserObject> Users { get; set; }

        public DbSet<ZugewieseneAufgabe> ZugewieseneAufgaben { get; set; }

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
