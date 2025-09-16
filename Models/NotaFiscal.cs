using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace xml_reader.Models
{
    [XmlRoot(ElementName = "NotaFiscal")]
    public class NotaFiscal
    {
        public int IdNotaFiscal { get; set; }

        [XmlElement(ElementName = "Numero")]
        public string Numero { get; set; }

        public string PrestadorCNPJ { get; set; }
        public string TomadorCNPJ { get; set; }
        public string ServicoDescricao { get; set; }
        public string ServicoValor { get; set; }

        [XmlElement(ElementName = "DataEmissao")]
        public string DataEmissao { get; set; }

        public string? NomeArquivo { get; set; }

        [XmlElement(ElementName = "Prestador")]
        public Prestador Prestador 
        { 
            get => new Prestador { CNPJ = PrestadorCNPJ };
            set => PrestadorCNPJ = value?.CNPJ;
        }

        [XmlElement(ElementName = "Tomador")]
        public Tomador Tomador 
        { 
            get => new Tomador { CNPJ = TomadorCNPJ };
            set => TomadorCNPJ = value?.CNPJ;
        }

        [XmlElement(ElementName = "Servico")]
        public Servico Servico 
        { 
            get => new Servico { Descricao = ServicoDescricao, Valor = ServicoValor };
            set 
            { 
                ServicoDescricao = value?.Descricao;
                ServicoValor = value.Valor;
            }
        }
    }

}