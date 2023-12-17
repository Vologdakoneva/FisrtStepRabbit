using DocumentService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DocumentService.Data
{
    public class DocumentDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public DocumentDbContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("PostgreSQL"));
        }

        public  DbSet<DocAnaliz> DocAnaliz { get; set; }

    }
}
