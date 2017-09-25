using System;
using System.Threading.Tasks;

namespace SearchEngine.Infrastructure
{
    public interface IDocumentController
    {
        Task UpsertDocument<TDocument>(IUpsertDocumentContext<TDocument> context) where TDocument : class;
    }
}