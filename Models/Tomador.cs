using System.Xml.Serialization;

namespace xml_reader.Models
{
    [XmlRoot(ElementName = "Tomador")]
    public class Tomador
    {
        [XmlElement(ElementName = "CNPJ")]
        public string CNPJ { get; set; }
    }
}
