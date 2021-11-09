using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;
//Че использовать та ?

namespace PresentationService.Models
{
    public class PresentationContext : DbContext
    {
        public PresentationContext() : base("DefaultConnection")
        {
        }

        public DbSet<Presentation> Presentation { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Presentation>( ).ToTable("presentation");
            base.OnModelCreating(modelBuilder);
        }
    }
}
