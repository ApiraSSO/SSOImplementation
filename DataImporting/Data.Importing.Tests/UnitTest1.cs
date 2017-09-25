using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Importing.Infrastructure;
using Data.Importing.Infrastructure.Contexts;
using Data.Importing.Repositories;
using Data.Importing.StageProcessors;
using Kernel.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Data.Importing.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var processorTypes = ReflectionHelper.GetAllTypes(new[] { this.GetType().Assembly }, 
                t => !t.IsAbstract && !t.IsInterface && typeof(IStageProcessor<ImportStages>).IsAssignableFrom(t))
                .ToList();
            var instances = processorTypes.Select(x => Activator.CreateInstance(x, new Action(() => { })) as StageProcessor)
                .OrderByDescending(o => o.Stage)
                .ToList();

            var seed = new Func<ImportContext, Task<StageResultContext>>(async c =>
            {
                var result = await c.SourceContext.Source;
                return new StageResultContext(null, c, null);
            });
            var stageResult = new StageResult(new RamRepository());
            var resultcontext = new StageResultContext(stageResult, null, null);
            var sourceContext = new SourceContext(() => Task.FromResult(resultcontext));
            var context = new ImportContext(sourceContext, null);
            //var stageContext = new ImportContext(null, context);
            var del = instances.Aggregate(seed, (f, next) => new Func<ImportContext, Task<StageResultContext>>(c => next.GetResultAsync(c, f)));
            var res = del(context);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var processorTypes = ReflectionHelper.GetAllTypes(new[] { this.GetType().Assembly },
                t => !t.IsAbstract && !t.IsInterface && typeof(IStageProcessor<ImportStages>).IsAssignableFrom(t))
                .ToList();
            var instances = processorTypes.Select(x => Activator.CreateInstance(x, new Action(() => { })) as StageProcessor)
                .OrderByDescending(o => o.Stage)
                .ToList();

            var seed = new Func<ImportContext, Task<StageResultContext>>(async c =>
            {
                var result = await c.SourceContext.Source;
                return new StageResultContext(null, c, null);
            });
            var stageResult = new StageResult(new RamRepository());
            var resultcontext = new StageResultContext(stageResult, null, null);
            var sourceContext = new SourceContext(() => Task.FromResult(resultcontext));
            var context = new ImportContext(sourceContext, null);
            //var stageContext = new ImportContext(null, context);
            var del = instances.Aggregate(seed, (f, next) => new Func<ImportContext, Task<StageResultContext>>(async c =>
            {
                var result = await c.SourceContext.Source;
                ////var result = await this.GetResultAsync(context);
                if (result.IsCompleted)
                    return result;
                var nextSourceContext = new SourceContext(() => next.GetResultAsync(c));
                return await f(new ImportContext(nextSourceContext, new TargetContext()));
                //throw new NotImplementedException();
            }));
            var res = del(context);
        }
    }
}