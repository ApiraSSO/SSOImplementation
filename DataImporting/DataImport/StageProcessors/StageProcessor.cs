using System;
using System.Threading.Tasks;
using Data.Importing.Infrastructure;
using Data.Importing.Infrastructure.Contexts;
using Kernel.DependancyResolver;

namespace Data.Importing.StageProcessors
{
    internal abstract class StageProcessor : IStageProcessor<ImportStages>
    {
        protected readonly IDependencyResolver DependencyResolver;

        public abstract ImportStage<ImportStages> Stage { get; }

        public StageProcessor(IDependencyResolver dependencyResolver)
        {
            this.DependencyResolver = dependencyResolver;
        }

        public StageResultContext GetResult(ImportContext context)
        {
            var result = Task.Factory.StartNew<Task<StageResultContext>>(async () => await this.GetResultAsync(context));
            result.Wait();
            return result.Result.Result;
        }

        public async Task<StageResultContext> GetResultAsync(ImportContext context, Func<ImportContext, Task<StageResultContext>> next)
        {
            //var result = await context.SourceContext.Source;
            ////var result = await this.GetResultAsync(context);
            //if (result.IsCompleted)
            //    return result;
            var sourceContext = new SourceContext(() => this.GetResultAsync(context));
            return await next(new ImportContext(sourceContext, new TargetContext()));
        }

        public async Task<StageResultContext> GetResultAsync(ImportContext context)
        {
            var source = await context.SourceContext.Source;
            //var result = await this.GetResultAsync(context);
            if (source.IsCompleted)
                return source;
            var result = await this.GetResultAsyncInternal(context);
            return new StageResultContext(result, context, this);

        }

        protected abstract Task<StageResult> GetResultAsyncInternal(ImportContext context);
    }
}