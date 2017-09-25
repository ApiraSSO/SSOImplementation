namespace Data.Importing.Infrastructure.Contexts
{
    public class StageResultContext
    {
        //private Lazy<Task<StageResultContext>> _lazyResult;
        private ImportContext ImportContext;
        private IStageProcessor StageProcessor;
        
        public StageResult Result { get; private set; }

        public bool IsCompleted
        {
            get
            {
                return this.Result.IsCompleted;
            }
        }

        public StageResultContext(StageResult result, ImportContext importContext, IStageProcessor stageProcessor)
        {
            this.ImportContext = importContext;
            this.StageProcessor = stageProcessor;
            this.Result = result;
            //var stageContext = new StageImportContext(result, importContext);
            //this._lazyResult = new Lazy<Task<StageResultContext>>(new Func<Task<StageResultContext>>(() => stageProcessor.GetResultAsync(stageContext)));
        }
    }
}