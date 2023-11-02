using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PacientService.Entities;
using PacientService.Repositories.Interfaces;
//using PromedExchange;

namespace PacientService.Data
{


    public class PacientDbContext : DbContext
    {
        protected readonly IConfiguration _configuration;

        public PacientDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Setups>()
                .HasData(new Setups()
                {
                    IDALL = 1,
                    Namenastr = "URL_PACIENT",
                    NamenRus = "Url для сервиса пациенты",
                    ValueString = "http://localhost:39289/api/Pacient"
                });

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // // // "Server = 127.0.0.1; Port = 5432; Database = postgres; User Id = postgres; Password = postgres; Pooling = true"

            //optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PostgreSQL"));
            optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=postgres;User Id=postgres;Password=postgres;Pooling=true");
            
        }



        public DbSet<Setups> Setups { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<ErrorPerson> ErrorPerson { get; set; }

    }
}
