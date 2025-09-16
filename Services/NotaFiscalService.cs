using Microsoft.EntityFrameworkCore;
using xml_reader.Data;
using xml_reader.Models;

namespace xml_reader.Services
{
    public class NotaFiscalService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NotaFiscalService> _logger;

        public NotaFiscalService(ApplicationDbContext context, ILogger<NotaFiscalService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<NotaFiscal>> BuscarNotasFiscais()
        {
            return await _context.NotasFiscais
                .OrderByDescending(nf => nf.Numero)
                .ToListAsync();
        }

        public async Task<NotaFiscal?> BuscarNotaFiscalAsync(int id)
        {
            return await _context.NotasFiscais.FindAsync(id);
        }

        public async Task<List<NotaFiscal>> SalvarNotasFiscaisAsync(List<NotaFiscal> notasFiscais)
        {
            var notasFiscaisSalvas = new List<NotaFiscal>();

            foreach (var notaFiscal in notasFiscais)
            {
                try
                {
                    var notaFiscalExistente = await _context.NotasFiscais
                        .FirstOrDefaultAsync(nf =>
                            nf.Numero == notaFiscal.Numero &&
                            nf.PrestadorCNPJ == notaFiscal.PrestadorCNPJ);

                    if (notaFiscalExistente != null)
                    {
                        _logger.LogWarning($"Nota fiscal já existe: {notaFiscalExistente.Numero}");
                        continue;
                    }

                    _context.NotasFiscais.Add(notaFiscal);
                    notasFiscaisSalvas.Add(notaFiscal);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Erro ao salvar nota fiscal: {notaFiscal.Numero}");
                    throw;
                }
            }

            if (notasFiscaisSalvas.Any())
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Salvadas {notasFiscaisSalvas.Count} notas fiscais no banco de dados");
            }

            return notasFiscaisSalvas;
        }

        public async Task<List<NotaFiscal>> ProcurarNotasFiltroAsync(string? Numero = null, string? PrestadorCNPJ = null, string? TomadorCNPJ = null, DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            var query = _context.NotasFiscais.AsQueryable();

            if (!string.IsNullOrEmpty(Numero))
                query = query.Where(nf => nf.Numero.Contains(Numero));

            if (!string.IsNullOrEmpty(PrestadorCNPJ))
            {
                var cnpjLimpo = new string(PrestadorCNPJ.Where(char.IsDigit).ToArray());
                query = query.Where(nf => nf.PrestadorCNPJ.Contains(cnpjLimpo));
            }

            if (!string.IsNullOrEmpty(TomadorCNPJ))
            {
                var cnpjLimpo = new string(TomadorCNPJ.Where(char.IsDigit).ToArray());
                query = query.Where(nf => nf.TomadorCNPJ.Contains(cnpjLimpo));
            }

            if (dataInicio.HasValue)
                query = query.Where(nf => DateTime.Parse(nf.DataEmissao) >= dataInicio.Value);

            if (dataFim.HasValue)
                query = query.Where(nf => DateTime.Parse(nf.DataEmissao) <= dataFim.Value);

            return await query
                .OrderByDescending(nf => nf.DataEmissao)
                .ToListAsync();
        }

        public async Task<decimal> BuscarValorPelaDataAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _context.NotasFiscais
                .Where(nf => DateTime.Parse(nf.DataEmissao) >= dataInicio && DateTime.Parse(nf.DataEmissao) <= dataFim)
                .SumAsync(nf => decimal.Parse(nf.ServicoValor));
        }

        public async Task<int> BuscarQuantidadeDeNotasAsync(string PrestadorCNPJ)
        {
            return await _context.NotasFiscais
                .CountAsync(nf => nf.PrestadorCNPJ == PrestadorCNPJ);
        }
    }
}