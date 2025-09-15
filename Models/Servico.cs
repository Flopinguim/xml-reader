using System.Xml.Serialization;

namespace xml_reader.Models
{
    [XmlRoot(ElementName = "Servico")]
    public class Servico
    {
        [XmlElement(ElementName = "Descricao")]
        public string Descricao { get; set; }
        [XmlElement(ElementName = "Valor")]
        public string Valor { get; set; }
    }
}
