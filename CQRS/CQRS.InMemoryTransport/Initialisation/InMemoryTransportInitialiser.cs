using System;
using System.Threading.Tasks;
using Kernel.DependancyResolver;
using Shared.Initialisation;

namespace CQRS.InMemoryTransport.Initialisation
{
    internal class InMemoryTransportInitialiser : Initialiser
    {
        public override byte Order => throw new NotImplementedException();

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            return Task.CompletedTask;
        }
    }
}