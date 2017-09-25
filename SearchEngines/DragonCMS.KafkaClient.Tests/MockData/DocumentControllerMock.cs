using System.Threading.Tasks;
using DragonCMS.Common.Registration;
using DragonCMS.Common.SearchEngine;

namespace DragonCMS.KafkaClient.Tests.MockData
{
    /// <summary>
    /// Handles elastic search document menagement such as indecies, updates etc
    /// </summary>
    internal class DocumentControllerMock : IDocumentController, IAutoRegisterAsTransient
    {
        private readonly IDocumentDispatcher _documentDispatcher;
        //private readonly IIndexManager _indexManager;
        //private readonly IResponseHandler _responseHandler;

        public DocumentControllerMock(IDocumentDispatcher documentDispatcher)
        {
            this._documentDispatcher = documentDispatcher;
            //this._indexManager = indexManager;
            //this._responseHandler = responseHandler;
        }

        public virtual async Task UpsertDocument<TDocument>(IUpsertDocumentContext<TDocument> context) where TDocument : class
        {
            await this._documentDispatcher.UpsertDocument<TDocument>(context);
        }
    }
}