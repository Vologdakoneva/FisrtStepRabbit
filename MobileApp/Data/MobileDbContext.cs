using Microsoft.EntityFrameworkCore;
using MobileApp.Entities;

namespace MobileApp.Data
{
    public class MobileDbContext : DbContext
    {
        protected readonly IConfiguration _configuration;

        public MobileDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // // // "Server = 127.0.0.1; Port = 5432; Database = postgres; User Id = postgres; Password = postgres; Pooling = true"

            //optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PostgreSQL"));
            // "Server=127.0.0.1;Port=5432;Database=postgres;User Id=postgres;Password=postgres;Pooling=true"
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PostgreSQL"));

        }
        public DbSet<UserApp> UsersApp { get; set; }
    }
}
