using Colt.Application.Interfaces;
using DocumentFormat.OpenXml.Packaging;
using GrapeCity.Documents.Word;

namespace Colt.Application.Services
{
    public class DocumentService : IDocumentService
    {
        public void ProcessFile<T>(T model, Stream fileStram, string outputPath) where T : class
        {
            var doc = new GcWordDocument();
            doc.Load(fileStram);

            doc.DataTemplate.DataSources.Add("ds", model);

            doc.DataTemplate.Process();

            doc.Save(outputPath);

            RemoveParagraph(outputPath);
        }

        private void RemoveParagraph(string outputPath)
        {
            using (var wordDoc = WordprocessingDocument.Open(outputPath, true))
            {
                var mainPart = wordDoc.MainDocumentPart;
                var doc = mainPart.Document;
                var paragraphs = doc.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().ToList();

                paragraphs[0].Remove();

                doc.Save();
            }
        }
    }
}
