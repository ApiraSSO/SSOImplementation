using Data.Importing.Infrastructure;
using Data.Importing.Infrastructure.Contexts;

namespace Data.Importing.StageResultContexts
{
    public class ContentStageResult<T> : StageResultContext
    {
        private readonly T _result;

        public T Result
        {
            get
            {
                return this._result;
            }
        }
        public ContentStageResult(ImportContext importContext, IStageProcessor stageProcessor, T result) 
            : base(importContext, stageProcessor)
        {
            this._result = result;
        }
    }
}