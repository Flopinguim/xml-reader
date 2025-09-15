using System.Xml.Serialization;

namespace xml_reader.Models
{
    [XmlRoot(ElementName = "Prestador")]
    public class Prestador
    {
        [XmlElement(ElementName = "CNPJ")]
        public string CNPJ { get; set; }
    }
}
