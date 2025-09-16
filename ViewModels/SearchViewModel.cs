using System.ComponentModel.DataAnnotations;

namespace xml_reader.ViewModels
{
    public class SearchViewModel
    {
        [Display(Name = "Número da Nota")]
        public string? NumeroNota { get; set; }

        [Display(Name = "CNPJ do Prestador")]
        public string? CnpjPrestador { get; set; }

        [Display(Name = "CNPJ do Tomador")]
        public string? CnpjTomador { get; set; }

        [Display(Name = "Data de Início")]
        [DataType(DataType.Date)]
        public DateTime? DataInicio { get; set; }

        [Display(Name = "Data de Fim")]
        [DataType(DataType.Date)]
        public DateTime? DataFim { get; set; }

        public List<xml_reader.Models.NotaFiscal> Results { get; set; } = new List<xml_reader.Models.NotaFiscal>();
    }
}
