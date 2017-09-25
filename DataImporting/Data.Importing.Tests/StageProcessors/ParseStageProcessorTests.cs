using System;
using Data.Importing.Infrastructure;
using Data.Importing.Infrastructure.Contexts;
using Data.Importing.Repositories;
using Data.Importing.StageProcessors;
using Data.Importing.Tests.MockData.DependencyResolvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serialisation.JSON;
using Serialisation.JSON.SettingsProviders;

namespace Data.Importing.Tests.StageProcessors
{
    internal class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [TestClass]
    public class ParseStageProcessorTests
    {

        [TestMethod]
        [Ignore]
        public void TestMethod1()
        {
            throw new NotImplementedException();
            ////ARRANGE
            //var resolver = new MockDependencyResolver();
            //var setting = new DefaultSettingsProvider();
            //var ser = new NSJsonSerializer(setting);
            //var processor = new ParseStageProcessor(resolver, ser);
            //var targetContext = new TargetContext { TargetType = typeof(Test) };
            //var importContext = new ImportContext(null, targetContext);
            //var o = ser.Serialize(new Test { Id = 1, Name = "Test" });
            ////var result = new StageResult(new RamRepository(new[] { new ImportedEntry(0) }));
            //var sourceContext = new SourceContext(() => new RamRepository(new[] { new ImportedEntry(0) }));
            ////result.Validated();
            //var stageContext = new ImportContext(sourceContext, targetContext);
            ////ACT
            //var r = processor.GetResultAsync(stageContext).Result;
            ////ASSERT
        }
    }
}
