using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using xml_reader.Models;

namespace xml_reader.Services
{
    public class ArquivoXmlService
    {
        public ArquivoXmlService()
        {
        }

        public async Task<List<NotaFiscal>> ProcessarArquivosXmlAsync(List<IFormFile> arquivoXml)
        {
            var notasFiscais = new List<NotaFiscal>();

            foreach (var file in arquivoXml)
            {
                try
                {
                    var notaFiscal = ProcessarArquivoXml(file);

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

        public NotaFiscal? ProcessarArquivoXml(IFormFile arquivoXml)
        {
            if (arquivoXml == null || arquivoXml.Length == 0)
            {
                return null;
            }

            try
            {
                using var stream = arquivoXml.OpenReadStream();

                XmlSerializer serializer = new XmlSerializer(typeof(NotaFiscal));

                NotaFiscal notaFiscal = (NotaFiscal?)serializer.Deserialize(stream) ?? throw new InvalidOperationException("Não foi possível deserializar a nota.");
                notaFiscal.NomeArquivo = arquivoXml.FileName;

                return notaFiscal;

            }
            catch (XmlException ex)
            {
                throw new InvalidOperationException($"Arquivo XML inválido: {arquivoXml.FileName}");
            }
        }
    }
}