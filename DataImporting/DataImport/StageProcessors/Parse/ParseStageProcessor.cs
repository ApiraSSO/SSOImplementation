using System;
using System.Threading.Tasks;
using Data.Importing.Infrastructure;
using Data.Importing.Infrastructure.Contexts;
using Kernel.DependancyResolver;
using Kernel.Serialisation;

namespace Data.Importing.StageProcessors
{
    internal class ParseStageProcessor : StageProcessor
    {
        private readonly ISerializer _serializer;

        public  override ImportStage<ImportStages> Stage
        {
            get
            {
                return new ImportStage<ImportStages>(ImportStages.Deserialise);
            }
        }

        public ParseStageProcessor(IDependencyResolver dependencyResolver, ISerializer serializer) : base(dependencyResolver)
        {
            this._serializer = serializer;
        }
        protected override Task<StageResult> GetResultAsyncInternal(ImportContext context)
        {
            throw new NotImplementedException();
        }
    }
}