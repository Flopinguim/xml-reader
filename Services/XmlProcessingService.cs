using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using xml_reader.Models;

namespace xml_reader.Services
{
    public class XmlProcessingService
    {
        public XmlProcessingService()
        {
        }

        public async Task<List<NotaFiscal>> ProcessXmlFilesAsync(List<IFormFile> xmlFiles)
        {
            var notasFiscais = new List<NotaFiscal>();

            foreach (var file in xmlFiles)
            {
                try
                {
                    var notaFiscal = await ProcessXmlFileAsync(file);

                    if (notaFiscal != null)
                    {
                        notasFiscais.Add(notaFiscal);
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Erro ao processar o arquivo {file.FileName}: {ex.Message}");
                }
            }

            return notasFiscais;
        }

        public async Task<NotaFiscal?> ProcessXmlFileAsync(IFormFile xmlFile)
        {
            if (xmlFile == null || xmlFile.Length == 0)
            {
                return null;
            }

            try
            {
                using var stream = xmlFile.OpenReadStream();

                XmlSerializer serializer = new XmlSerializer(typeof(NotaFiscal));

                using (FileStream fs = new FileStream(xmlFile.Name, FileMode.Open))
                {
                    NotaFiscal? notaFiscal = (NotaFiscal?)serializer.Deserialize(fs);
                }

                    var document = await XDocument.LoadAsync(stream, LoadOptions.None, CancellationToken.None);

                return ExtractNotaFiscalData(document, xmlFile.FileName);
            }
            catch (XmlException ex)
            {
                throw new InvalidOperationException($"Arquivo XML inválido: {xmlFile.FileName}");
            }
        }

        private NotaFiscal ExtractNotaFiscalData(XDocument document, string fileName)
        {
            try
            {
                var root = document.Root;
                if (root == null)
                    throw new InvalidOperationException("XML não possui elemento raiz");

                var notaElement = root;

                notaElement = root.Descendants().FirstOrDefault(x => x.Name.LocalName.Equals("NotaFiscal"));

                var notaFiscal = new NotaFiscal
                {
                    NomeArquivo = fileName
                };

                //notaFiscal.NumeroNota = ExtractValue(notaElement, "Numero") ?? throw new InvalidOperationException("Número da nota não encontrado");

                //notaFiscal.CnpjPrestador = ExtractValue(notaElement, "Prestador/CNPJ") ?? throw new InvalidOperationException("CNPJ do prestador não encontrado");

                //notaFiscal.CnpjTomador = ExtractValue(notaElement, "Tomador/CNPJ") ?? throw new InvalidOperationException("CNPJ do tomador não encontrado");

                //notaFiscal.DataEmissao = DateTime.Parse(ExtractValue(notaElement, "DataEmissao"));

                //notaFiscal.DescricaoServico = ExtractValue(notaElement, "Servico/Descricao") ?? throw new InvalidOperationException("Descrição do serviço não encontrada");

                //notaFiscal.ValorTotal = Decimal.Parse(ExtractValue(notaElement, "Servico/Valor"));

                return notaFiscal;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao processar dados do XML: {ex.Message}");
            }
        }

        private string? ExtractValue(XElement element, params string[] xpaths)
        {
            foreach (var xpath in xpaths)
            {
                try
                {
                    var parts = xpath.Split('/');
                    var current = element;

                    foreach (var part in parts)
                    {
                        current = current?.Elements().FirstOrDefault(x =>
                            string.Equals(x.Name.LocalName, part, StringComparison.OrdinalIgnoreCase));

                        if (current == null) break;
                    }

                    if (current != null && !string.IsNullOrWhiteSpace(current.Value))
                    {
                        return current.Value.Trim();
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Erro ao buscar xpath: {xpath}", ex);
                }
            }

            return null;
        }
    }
}