using System;
using System.Linq;
using CQRS.MessageHandling.Factories;
using CQRS.MessageHandling.Invocation;
using CQRS.MessageHandling.Test.MockData;
using CQRS.MessageHandling.Test.MockData.MessageHandling;
using Kernel.DependancyResolver;
using NUnit.Framework;

namespace CQRS.MessageHandling.Test.MessageHandling
{


    [TestFixture]
    public class HandlerFactoryTests
    {
        [Test]
        public void HandlerDelegateFactoryTest()
        {
            //ARRANGE
            var result = 0;
            var command = new TestCommand(Guid.NewGuid(), Guid.NewGuid());
            var handler = new TestCommandHandler(() => result = 10);
            
            //ACT
            var del = HandlerDelegateFactory.GetdMessageHandlerDelegate(typeof(TestCommandHandler), typeof(TestCommand));
            del(handler, new[] { command });
            
            //ASSERT
            Assert.AreEqual(10, result);
        }

        [Test]
        public void CommandHandlerResolverTest_resolve_by_assembly_scanning_no_limit()
        {
            //ARRANGE
            var dependencyResolver = new DependencyResolverMock();
            var handlerFactorySettings = new HandlerFactorySettingsMock();
            var handlerResolver = new HandlerResolver(dependencyResolver, handlerFactorySettings);
            dependencyResolver.RegisterFactory<Action>(t => () => { }, Lifetime.Singleton);

            //ACT
            var handler = handlerResolver.ResolveAllHandlersFor(typeof(TestCommand));
            
            //ASSERT
            Assert.IsInstanceOf<TestCommandHandler>(handler.Single());
        }
        [Test]
        public void CommandHandlerResolverTest_resolve_by_assembly_scanning_limit_to_current_assembly()
        {
            //ARRANGE
            var dependencyResolver = new DependencyResolverMock();
            var handlerFactorySettings = new HandlerFactorySettingsMock();
            handlerFactorySettings.ClearList();
            handlerFactorySettings.AddAssembly(this.GetType().Assembly);
            var handlerResolver = new HandlerResolver(dependencyResolver, handlerFactorySettings);
            dependencyResolver.RegisterFactory<Action>(t => () => { }, Lifetime.Singleton);

            //ACT
            var handler = handlerResolver.ResolveAllHandlersFor(typeof(TestCommand));

            //ASSERT
            Assert.IsInstanceOf<TestCommandHandler>(handler.Single());
        }
        
        [Test]
        public void EventHandlerResolverTest()
        {
            //ARRANGE
            var dependencyResolver = new DependencyResolverMock();
            var handlerFactorySettings = new HandlerFactorySettingsMock();
            var handlerResolver = new HandlerResolver(dependencyResolver, handlerFactorySettings);
            dependencyResolver.RegisterFactory<Action>(t => () => { }, Lifetime.Singleton);

            //ACT
            var handler = handlerResolver.ResolveAllHandlersFor(typeof(TestEvent));

            //ASSERT
            Assert.IsInstanceOf<TestEventHandler>(handler.Single());
        }
    }
}
