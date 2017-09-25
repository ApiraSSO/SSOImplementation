using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kernel.CQRS.MessageHandling;
using Kernel.Initialisation;

namespace CQRS.MessageHandling.Invocation
{
    internal class HandlerInvoker : IHandlerInvoker, IAutoRegisterAsTransient
    {
        public Task InvokeHandlers(IEnumerable<object> handlers, object message)
        {
            return this.InvokeHandlersInternal(handlers, message);
        }

        private Task InvokeHandlersInternal(IEnumerable<object> handlers, object message)
        {
            if (handlers == null)
                throw new ArgumentNullException("message");

            var tasks = new List<Task>();

            var asList = handlers.ToList();

            asList.Aggregate(tasks, (c, handler) =>
            {
                var del = HandlerDelegateFactory.GetdMessageHandlerDelegate(handler.GetType(), message.GetType());
                var delTask = del(handler, new[] { message });
                tasks.Add(delTask);
                return c;
            });

            var whenAllTask = Task.WhenAll(tasks)
                .ContinueWith(t =>
                {
                    foreach (var h in asList)
                    {
                        var disposable = h as IDisposable;
                        disposable?.Dispose();
                    }

                    if (t.Exception != null)
                        throw t.Exception;
                });

            return whenAllTask;
        }
    }
}