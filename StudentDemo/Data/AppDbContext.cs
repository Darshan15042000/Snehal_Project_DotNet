using Microsoft.EntityFrameworkCore;
using StudentDemo.Models;

namespace StudentDemo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Admin> Students{ get; set; }
        public DbSet<SupportClient> SupportClients { get; set; }
        public DbSet<Developer> Developer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .ToTable("Admin");

            base.OnModelCreating(modelBuilder);
        }
    }
}