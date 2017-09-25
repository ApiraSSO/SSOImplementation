using System;
using System.Collections.Generic;

namespace SearchEngine.Infrastructure
{
    public interface IUpsertDocumentContext<T> where T : class
    {
        T Document { get; set; }

        Guid Id { get; }

        IndexContext IndexContext { get; set; }

        dynamic PartialUpdate { get; set; }

        string Script { get; set; }

        IDictionary<string, object> ScriptParams { get; }
    }
}