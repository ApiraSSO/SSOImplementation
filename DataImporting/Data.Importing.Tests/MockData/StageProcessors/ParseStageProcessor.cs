using System;
using System.Threading.Tasks;
using Data.Importing.Infrastructure;
using Data.Importing.Infrastructure.Contexts;
using Data.Importing.Tests.MockData.DependencyResolvers;

namespace Data.Importing.Tests.MockData.StageProcessors
{
    internal class ParseStageProcessor : MockStageProcessor
    {
        

        public  override ImportStage<ImportStages> Stage
        {
            get
            {
                return new ImportStage<ImportStages>(ImportStages.Deserialise);
            }
        }

        public ParseStageProcessor(Action action) : base(new MockDependencyResolver(), action)
        {
            
        }
        protected override Task<StageResult> GetResultAsyncInternal(ImportContext context)
        {
            base.Action();
            return Task.FromResult(new StageResult(null));
        }
    }
}