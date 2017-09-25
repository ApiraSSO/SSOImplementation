using System.Threading.Tasks;

namespace SearchEngine.Infrastructure
{
    public interface IDocumentDispatcher
    {
        Task UpsertDocument<TDocument>(IUpsertDocumentContext<TDocument> context) where TDocument : class;
    }
}