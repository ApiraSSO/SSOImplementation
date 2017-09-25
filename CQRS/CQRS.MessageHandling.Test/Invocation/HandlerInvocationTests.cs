using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CQRS.MessageHandling.Invocation;
using CQRS.MessageHandling.Test.MockData.MessageHandling;
using NUnit.Framework;

namespace CQRS.MessageHandling.Test.Invocation
{
    [TestFixture]
    internal class HandlerInvocationTests
    {
        [Test]
        public async Task HandlerInvokerTest()
        {
            //ARRANGE
            var result = 0;
            var command = new TestCommand(Guid.NewGuid(), Guid.NewGuid());
            var handler = new TestCommandHandler(() => result = 10);
            var handlerInvoker = new HandlerInvoker();
            //ACT
            await handlerInvoker.InvokeHandlers(new[] { handler }, command);

            //ASSERT
            Assert.AreEqual(10, result);
        }
    }
}
