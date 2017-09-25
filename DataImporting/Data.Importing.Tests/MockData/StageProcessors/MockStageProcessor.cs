using System;
using Data.Importing.StageProcessors;
using Kernel.DependancyResolver;

namespace Data.Importing.Tests.MockData.StageProcessors
{
    internal abstract class MockStageProcessor : StageProcessor
    {
        protected readonly  Action Action;
        public MockStageProcessor(IDependencyResolver dependencyResolver, Action action) : base(dependencyResolver)
        {
            this.Action = action;
        }
    }
}