using BugTracker.Entity;
using Microsoft.EntityFrameworkCore;

namespace BugTracker
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PersonProject>()
                .HasKey(pp => new { pp.PersonId, pp.ProjectId});
        }

        public DbSet<Person> Personnel { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<PersonProject> PersonnelProjects { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
    }
}
