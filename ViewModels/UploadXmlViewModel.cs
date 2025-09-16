using System.ComponentModel.DataAnnotations;

namespace xml_reader.ViewModels
{
    public class UploadXmlViewModel
    {
        [Required(ErrorMessage = "Selecione pelo menos um arquivo XML")]
        [Display(Name = "Arquivos XML")]
        public List<IFormFile> ArquivosXml { get; set; } = new List<IFormFile>();

        public List<string> ProcessamentoResultado { get; set; } = new List<string>();
        public bool PossuiErros { get; set; }
        public List<string> MensagensErro { get; set; } = new List<string>();
    }
}
