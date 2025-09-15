//using Microsoft.EntityFrameworkCore;
//using xml_reader.Data;
//using xml_reader.Models;

//namespace xml_reader.Services
//{
//    public class NotaFiscalService
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly ILogger<NotaFiscalService> _logger;

//        public NotaFiscalService(ApplicationDbContext context, ILogger<NotaFiscalService> logger)
//        {
//            _context = context;
//            _logger = logger;
//        }

//        public async Task<List<NotaFiscal>> GetAllAsync()
//        {
//            return await _context.NotasFiscais
//                .OrderByDescending(nf => nf.DataProcessamento)
//                .ToListAsync();
//        }

//        public async Task<NotaFiscal?> GetByIdAsync(int id)
//        {
//            return await _context.NotasFiscais.FindAsync(id);
//        }

//        public async Task<List<NotaFiscal>> SaveNotasFiscaisAsync(List<NotaFiscal> notasFiscais)
//        {
//            var savedNotasFiscais = new List<NotaFiscal>();
            
//            foreach (var notaFiscal in notasFiscais)
//            {
//                try
//                {
//                    // Check if invoice already exists
//                    var existing = await _context.NotasFiscais
//                        .FirstOrDefaultAsync(nf => 
//                            nf.NumeroNota == notaFiscal.NumeroNota && 
//                            nf.CnpjPrestador == notaFiscal.CnpjPrestador);

//                    if (existing != null)
//                    {
//                        _logger.LogWarning("Nota fiscal já existe: {NumeroNota} - {CnpjPrestador}", 
//                            notaFiscal.NumeroNota, notaFiscal.CnpjPrestador);
//                        continue;
//                    }

//                    notaFiscal.DataProcessamento = DateTime.Now;
//                    _context.NotasFiscais.Add(notaFiscal);
//                    savedNotasFiscais.Add(notaFiscal);
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogError(ex, "Erro ao salvar nota fiscal: {NumeroNota}", notaFiscal.NumeroNota);
//                    throw;
//                }
//            }

//            if (savedNotasFiscais.Any())
//            {
//                await _context.SaveChangesAsync();
//                _logger.LogInformation("Salvadas {Count} notas fiscais no banco de dados", savedNotasFiscais.Count);
//            }

//            return savedNotasFiscais;
//        }

//        public async Task<List<NotaFiscal>> SearchAsync(string? numeroNota = null, 
//            string? cnpjPrestador = null, 
//            string? cnpjTomador = null,
//            DateTime? dataInicio = null,
//            DateTime? dataFim = null)
//        {
//            var query = _context.NotasFiscais.AsQueryable();

//            if (!string.IsNullOrEmpty(numeroNota))
//            {
//                query = query.Where(nf => nf.NumeroNota.Contains(numeroNota));
//            }

//            if (!string.IsNullOrEmpty(cnpjPrestador))
//            {
//                var cnpjLimpo = new string(cnpjPrestador.Where(char.IsDigit).ToArray());
//                query = query.Where(nf => nf.CnpjPrestador.Contains(cnpjLimpo));
//            }

//            if (!string.IsNullOrEmpty(cnpjTomador))
//            {
//                var cnpjLimpo = new string(cnpjTomador.Where(char.IsDigit).ToArray());
//                query = query.Where(nf => nf.CnpjTomador.Contains(cnpjLimpo));
//            }

//            if (dataInicio.HasValue)
//            {
//                query = query.Where(nf => nf.DataEmissao >= dataInicio.Value);
//            }

//            if (dataFim.HasValue)
//            {
//                query = query.Where(nf => nf.DataEmissao <= dataFim.Value);
//            }

//            return await query
//                .OrderByDescending(nf => nf.DataEmissao)
//                .ToListAsync();
//        }

//        public async Task<decimal> GetTotalValueByPeriodAsync(DateTime dataInicio, DateTime dataFim)
//        {
//            return await _context.NotasFiscais
//                .Where(nf => nf.DataEmissao >= dataInicio && nf.DataEmissao <= dataFim)
//                .SumAsync(nf => nf.ValorTotal);
//        }

//        public async Task<int> GetCountByPrestadorAsync(string cnpjPrestador)
//        {
//            return await _context.NotasFiscais
//                .CountAsync(nf => nf.CnpjPrestador == cnpjPrestador);
//        }
//    }
//}