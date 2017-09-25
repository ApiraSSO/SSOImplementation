using Data.Importing.Infrastructure.Configuration;

namespace Data.Importing.Infrastructure.Factories
{
    public interface IImporterFactory
    {
        IImporter GetImporter(ImportConfiguration configuration);
    }
}