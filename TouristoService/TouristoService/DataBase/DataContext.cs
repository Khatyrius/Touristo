using Microsoft.EntityFrameworkCore;
using TouristoService.Models;

namespace TouristoService.DataBase
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().HasMany(c => c.cities).WithOne(c => c.country);
            modelBuilder.Entity<City>().HasMany(c => c.attractions).WithOne(a => a.city);
            base.OnModelCreating(modelBuilder);
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
