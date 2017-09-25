using System.Threading.Tasks;
using ElasticSearchClient.ErrorHandling;
using ElasticSearchClient.IndexAPI;
using Kernel.Initialisation;
using SearchEngine.Infrastructure;

namespace ElasticSearchClient.DocumentAPI
{
    /// <summary>
    /// Handles elastic search document menagement such as indecies, updates etc
    /// </summary>
    internal class DocumentController : IDocumentController, IAutoRegisterAsTransient
    {
        private readonly IDocumentDispatcher _documentDispatcher;
        private readonly IIndexManager _indexManager;
        private readonly IResponseHandler _responseHandler;

        public DocumentController(IDocumentDispatcher documentDispatcher, IIndexManager indexManager, IResponseHandler responseHandler)
        {
            this._documentDispatcher = documentDispatcher;
            this._indexManager = indexManager;
            this._responseHandler = responseHandler;
        }

        public virtual async Task UpsertDocument<TDocument>(IUpsertDocumentContext<TDocument> context) where TDocument : class
        {
            await this._documentDispatcher.UpsertDocument<TDocument>(context);
        }
    }
}