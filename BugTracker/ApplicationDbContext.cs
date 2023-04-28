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

            modelBuilder.Entity<TicketPerson>()
                .HasKey(tp => new { tp.PersonId, tp.TicketId });

            modelBuilder.Entity<TicketPerson>()
                .HasOne(tp => tp.Ticket)
                .WithMany(t => t.AssignedPeople)
                .HasForeignKey(tp => tp.TicketId);

            modelBuilder.Entity<TicketPerson>()
                .HasOne(tp => tp.Person)
                .WithMany()
                .HasForeignKey(tp => tp.PersonId);
        }

        public DbSet<Person> Personnel { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<PersonProject> PersonnelProjects { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketPerson> TicketsPersonnel { get; set; }
    }
}
