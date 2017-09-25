using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Kernel.Reflection.Extensions;

namespace CQRS.MessageHandling.Invocation
{
    internal class HandlerDelegateFactory
    {
        private static readonly ConcurrentDictionary<Tuple<Type, Type>, Func<object, object[], Task>> MessageHandlerDelegatesCache = new ConcurrentDictionary<Tuple<Type, Type>, Func<object, object[], Task>>();

        public static Func<object, object[], Task> GetdMessageHandlerDelegate(Type handlerType, Type commandType)
        {
            return HandlerDelegateFactory.MessageHandlerDelegatesCache.GetOrAdd(new Tuple<Type, Type>(handlerType, commandType), t => HandlerDelegateFactory.BuildMessageHandlerDelegateInternal(handlerType, commandType));
        }

        private static Func<object, object[], Task> BuildMessageHandlerDelegateInternal(Type targetType, Type commandType)
        {
            return TypeExtensions.GetAsyncInvoker(targetType, "Handle", commandType);
            //var method = targetType.GetMethods().Single(c => c.Name == "Handle");
            //var targetParam = Expression.Parameter(typeof(object));
            //var eventParam = Expression.Parameter(typeof(object));

            //var handlerConvert = Expression.Convert(targetParam, targetType);
            //var commandConvert = Expression.Convert(eventParam, commandype);

            //var callExpression = Expression.Call(handlerConvert, method, commandConvert);
            //var lambda = Expression.Lambda<Func<object, object, Task>>(callExpression, targetParam, eventParam);
            //var compiledLambda = lambda.Compile();
            //return compiledLambda;
        }
    }
}