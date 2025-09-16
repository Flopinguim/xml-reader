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
                
                entity.HasKey(e => e.IdNotaFiscal);
                
                entity.Ignore(e => e.Prestador);
                entity.Ignore(e => e.Tomador);
                entity.Ignore(e => e.Servico);
                
                entity.Property(e => e.Numero).HasMaxLength(50);
                entity.Property(e => e.PrestadorCNPJ).HasMaxLength(18);
                entity.Property(e => e.TomadorCNPJ).HasMaxLength(18);
                entity.Property(e => e.ServicoDescricao).HasMaxLength(500);
                entity.Property(e => e.ServicoValor).HasMaxLength(20);
                entity.Property(e => e.DataEmissao).HasMaxLength(50);
                entity.Property(e => e.NomeArquivo).HasMaxLength(255);
            });
        }
    }
}