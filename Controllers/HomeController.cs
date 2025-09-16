using Microsoft.AspNetCore.Mvc;
using xml_reader.Models;
using xml_reader.Services;
using xml_reader.ViewModels;

namespace xml_reader.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ArquivoXmlService _xmlProcessingService;
        private readonly NotaFiscalService _notaFiscalService;

        public HomeController(ILogger<HomeController> logger,
            ArquivoXmlService xmlProcessingService,
            NotaFiscalService notaFiscalService)
        {
            _logger = logger;
            _xmlProcessingService = xmlProcessingService;
            _notaFiscalService = notaFiscalService;
        }

        public IActionResult Index()
        {
            return View(new UploadXmlViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(UploadXmlViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var notasFiscais = await _xmlProcessingService.ProcessarArquivosXmlAsync(model.ArquivosXml);

                if (notasFiscais.Any())
                {
                    var notasFiscaisSalvas = await _notaFiscalService.SalvarNotasFiscaisAsync(notasFiscais);

                    model.ProcessamentoResultado.Add($"Processados {notasFiscais.Count} arquivo(s) XML com sucesso.");
                    model.ProcessamentoResultado.Add($"Salvos {notasFiscaisSalvas.Count} nova(s) nota(s) fiscal(is) no banco de dados.");
                    
                    if (notasFiscaisSalvas.Count < notasFiscais.Count)
                    {
                        model.ProcessamentoResultado.Add($"{notasFiscais.Count - notasFiscaisSalvas.Count} nota(s) já existia(m) no banco de dados e foi(ram) ignorada(s).");
                    }
                }
                else
                {
                    model.PossuiErros = true;
                    model.MensagensErro.Add("Nenhuma nota fiscal válida foi encontrada nos arquivos XML.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar arquivos XML");
                model.PossuiErros = true;
                model.MensagensErro.Add($"Erro ao processar arquivos: {ex.Message}");
            }

            model.ArquivosXml = new List<IFormFile>();
            return View(model);
        }

        public async Task<IActionResult> Listar()
        {
            var notasFiscais = await _notaFiscalService.BuscarNotasFiscais();
            return View(notasFiscais);
        }

        public IActionResult Buscar()
        {
            return View(new SearchViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Buscar(SearchViewModel model)
        {
            try
            {
                model.Results = await _notaFiscalService.ProcurarNotasFiltroAsync(
                    model.NumeroNota,
                    model.CnpjPrestador,
                    model.CnpjTomador,
                    model.DataInicio,
                    model.DataFim);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar notas fiscais");
                ModelState.AddModelError("", $"Erro ao buscar notas fiscais: {ex.Message}");
            }

            return View(model);
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            var notaFiscal = await _notaFiscalService.BuscarNotaFiscalAsync(id);
            if (notaFiscal == null)
            {
                return NotFound();
            }

            return View(notaFiscal);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { });
        }
    }
}
