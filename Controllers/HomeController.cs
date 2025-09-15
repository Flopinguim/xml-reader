using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using xml_reader.Models;
using xml_reader.Services;
using xml_reader.ViewModels;

namespace xml_reader.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly XmlProcessingService _xmlProcessingService;
        private readonly NotaFiscalService _notaFiscalService;

        public HomeController(ILogger<HomeController> logger, 
            XmlProcessingService xmlProcessingService,
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
                // Validate file extensions
                var invalidFiles = model.XmlFiles.Where(f => !f.FileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase)).ToList();
                if (invalidFiles.Any())
                {
                    model.HasErrors = true;
                    model.ErrorMessages.Add($"Os seguintes arquivos n�o s�o XML v�lidos: {string.Join(", ", invalidFiles.Select(f => f.FileName))}");
                    return View(model);
                }

                // Process XML files
                var notasFiscais = await _xmlProcessingService.ProcessXmlFilesAsync(model.XmlFiles);
                
                if (notasFiscais.Any())
                {
                    // Save to database
                    var savedNotasFiscais = await _notaFiscalService.SaveNotasFiscaisAsync(notasFiscais);
                    
                    model.ProcessingResults.Add($"Processados {notasFiscais.Count} arquivo(s) XML com sucesso.");
                    model.ProcessingResults.Add($"Salvos {savedNotasFiscais.Count} nova(s) nota(s) fiscal(is) no banco de dados.");
                    
                    if (savedNotasFiscais.Count < notasFiscais.Count)
                    {
                        model.ProcessingResults.Add($"{notasFiscais.Count - savedNotasFiscais.Count} nota(s) j� existia(m) no banco de dados e foi(ram) ignorada(s).");
                    }
                }
                else
                {
                    model.HasErrors = true;
                    model.ErrorMessages.Add("Nenhuma nota fiscal v�lida foi encontrada nos arquivos XML.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar arquivos XML");
                model.HasErrors = true;
                model.ErrorMessages.Add($"Erro ao processar arquivos: {ex.Message}");
            }

            // Clear the files to avoid resubmission
            model.XmlFiles = new List<IFormFile>();
            return View(model);
        }

        public async Task<IActionResult> Listar()
        {
            var notasFiscais = await _notaFiscalService.GetAllAsync();
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
                model.Results = await _notaFiscalService.SearchAsync(
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
            var notaFiscal = await _notaFiscalService.GetByIdAsync(id);
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
