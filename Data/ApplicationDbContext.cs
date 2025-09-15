using Microsoft.EntityFrameworkCore;
using xml_reader.Models;

namespace xml_reader.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<NotaFiscal> NotasFiscais { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NotaFiscal>(entity =>
            {
                entity.ToTable("NotasFiscais");             
            });
        }
    }
}