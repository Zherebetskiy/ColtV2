namespace Colt.Application.Interfaces
{
    public interface IDocumentService
    {
        void ProcessFile<T>(T model, Stream fileStram, string outputPath) where T : class;
    }
}
