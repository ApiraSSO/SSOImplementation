using System;
using Kernel.Data.DataRepository;
using Kernel.Messaging.Response;

namespace Data.Importing.Infrastructure
{
    public class StageResult : AbstractResponse
    {
        public IReadOnlyRepository<ImportedEntry, Guid> Result { get; private set; }

        public bool IsCompleted { get; }

        public bool IsResultValid { get; private set; }

        public StageResult(IReadOnlyRepository<ImportedEntry, Guid> result)
        {
            this.Result = result;
        }

        public void Validated()
        {
            this.IsResultValid = true;
        }
    }
}