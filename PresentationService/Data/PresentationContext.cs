using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;

namespace PresentationService.Models
{
    public class PresentationContext : DbContext
    {
        public PresentationContext(DbContextOptions<PresentationContext> options) : base(options)
        {
        }

        public DbSet<Presentation> Presentation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Presentation>(p =>
            {
                p.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                p.Property(e => e.Name)
                    .HasColumnName("name");

                p.Property(e => e.Time)
                    .HasColumnName("time");

                p.Property(e => e.Status)
                    .HasColumnName("status");

                p.Property(e => e.Description)
                    .HasColumnName("description");

                p.Property(e => e.Visitorsid)
                    .HasColumnName("visitorsid");
            });

            //modelBuilder.Entity<Presentation>().ToTable("presentation");
        }
    }
}
