using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using Kernel.Federation.Tokens;

namespace Federation.Protocols.Tokens.Validation
{
    internal class ValidatorInvoker
    {
        private static readonly ConcurrentDictionary<Type, Action<object, object>> _cache = new ConcurrentDictionary<Type, Action<object, object>>();
        private readonly Func<Type, object> _dependencyResolver;

        public ValidatorInvoker(Func<Type, object> dependencyResolver)
        {
            this._dependencyResolver = dependencyResolver;
        }
        public void Validate(object clause)
        {
            if (clause == null)
                throw new ArgumentNullException("token clause");
            var validatorType = typeof(ITokenClauseValidator<>).MakeGenericType(clause.GetType());
            var validator = this.ResolveValidator(validatorType);
            var del = ValidatorInvoker._cache.GetOrAdd(clause.GetType(), k => this.BuildValidateDelegate(validatorType, k));
            del(validator, clause);
        }

        private Action<object, object> BuildValidateDelegate(Type validatorType, Type clauseType)
        {
            var targetExp = Expression.Parameter(typeof(object));
            var argExp = Expression.Parameter(typeof(object));
            var targetConvertExp = Expression.Convert(targetExp, validatorType);
            var argConvertExp = Expression.Convert(argExp, clauseType);

            var minfo = validatorType.GetMethod("Validate", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var callExp = Expression.Call(targetConvertExp, minfo, argConvertExp);
            var lambda = Expression.Lambda<Action<object, object>>(callExp, targetExp, argExp);
            return lambda.Compile();
        }

        private object ResolveValidator(Type type)
        {
            return this._dependencyResolver(type);
        }
    }
}