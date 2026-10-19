using Microsoft.EntityFrameworkCore;
using MuetStudentPortal.Models;

namespace MuetStudentPortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students => Set<Student>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure unique Roll Number
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.RollNumber)
                .IsUnique();
        }
    }
}