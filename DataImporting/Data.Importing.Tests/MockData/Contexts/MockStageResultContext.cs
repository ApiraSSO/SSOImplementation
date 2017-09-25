using Data.Importing.Infrastructure;
using Data.Importing.Infrastructure.Contexts;

namespace Data.Importing.Tests.MockData.Contexts
{
    public class MockStageResultContext : StageResultContext
    {
        public MockStageResultContext(StageImportContext importContext, IStageProcessor stageProcessor) : base(importContext, stageProcessor)
        {
        }
    }
}
