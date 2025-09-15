using System.Xml.Serialization;

namespace xml_reader.Models
{
    [XmlRoot(ElementName = "NotaFiscal")]
    public class NotaFiscal
    {
        [XmlElement(ElementName = "Numero")]
        public string Numero { get; set; }

        [XmlElement(ElementName = "Prestador")]
        public Prestador Prestador { get; set; }

        [XmlElement(ElementName = "Tomador")]
        public Tomador Tomador { get; set; }
        [XmlElement(ElementName = "DataEmissao")]

        public string DataEmissao { get; set; }

        [XmlElement(ElementName = "Servico")]
        [XmlElement(ElementName = "Servico")]
        public Servico Servico { get; set; }

        public string? NomeArquivo { get; set; }
    }

}