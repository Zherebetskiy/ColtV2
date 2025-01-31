namespace Colt.Application.Interfaces
{
    public interface IDocumentService
    {
        void ProcessFile<T>(T model, string inputPath, string outputPath) where T : class;
    }
}
