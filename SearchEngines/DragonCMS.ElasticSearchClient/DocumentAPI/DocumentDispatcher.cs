using System.Linq;
using System.Threading.Tasks;
using ElasticSearchClient.ErrorHandling;
using ElasticSearchClient.Factories;
using ElasticSearchClient.IndexAPI;
using Nest;
using SearchEngine.Infrastructure;

namespace ElasticSearchClient.DocumentAPI
{
    internal class DocumentDispatcher : IDocumentDispatcher
    {
        private readonly IClientFactory _clientFactory;
        private readonly IIndexManager _indexManager;
        private readonly IResponseHandler _responseHandler;

        public DocumentDispatcher(IClientFactory clientFactory, IIndexManager indexManager, IResponseHandler responseHandler)
        {
            this._clientFactory = clientFactory;
            this._indexManager = indexManager;
            this._responseHandler = responseHandler;
        }

        public virtual Task UpsertDocument<TDocument>(IUpsertDocumentContext<TDocument> context) where TDocument : class
        {
            var documentType = typeof(TDocument);
            var client = this._clientFactory.GetClient();

            var documentPath = new DocumentPath<TDocument>(context.Id)
                .Type(documentType);

            var index = this.GetIndex(context.IndexContext);
            var updateDescriptor = new UpdateDescriptor<TDocument, dynamic>(documentPath)
                .Index(index);

            if (context.Script != null)
                updateDescriptor.Script(d => d.Inline(context.Script)
                .Params(context.ScriptParams.ToDictionary(k => k.Key, v => v.Value)));
            else
                updateDescriptor.Upsert(context.Document)
                .Doc(context.PartialUpdate ?? context.Document);
            
            var updateDocResponse = client.Update<TDocument, dynamic>(updateDescriptor);

            this._responseHandler.ValdateAndHandleException(updateDocResponse, true);
            return Task.CompletedTask;
        }
        
        private IndexName GetIndex(IndexContext indexContext)
        {
            var index = this._indexManager.GetIndex(indexContext);
            return index.Result;
        }
    }
}