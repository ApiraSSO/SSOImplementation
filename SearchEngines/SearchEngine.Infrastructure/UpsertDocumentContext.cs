using System;
using System.Collections.Generic;

namespace SearchEngine.Infrastructure
{
    public class UpsertDocumentContext<TModel> : IUpsertDocumentContext<TModel>, ISearchable where TModel : class
    {
        public UpsertDocumentContext(Guid documentId)
        {
            this.Id = documentId;
            this.ScriptParams = new Dictionary<string, object>();
            this.IndexContext = new IndexContext(typeof(TModel));
        }
        public TModel Document { get; set; }

        public Guid Id { get; private set; }

        public IndexContext IndexContext { get; set; }

        public dynamic PartialUpdate { get; set; }

        public string Script { get; set; }

        public IDictionary<string, object> ScriptParams { get; private set; }
    }
}