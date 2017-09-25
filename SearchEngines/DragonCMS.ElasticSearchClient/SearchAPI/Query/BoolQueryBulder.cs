using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ElasticSearchClient.SearchAPI.Query.ClauseBuilders;
using Kernel.DependancyResolver;
using Kernel.Initialisation;
using Kernel.Reflection;
using Nest;
using SearchEngine.Infrastructure.Query;

namespace ElasticSearchClient.SearchAPI.Query
{
    internal class BoolQueryBulder<T> : IAutoRegisterAsTransient where T : class
    {
        private ConcurrentDictionary<Type, Func<object, object, Func<QueryContainerDescriptor<T>, QueryContainer>>> _delegateCache = new ConcurrentDictionary<Type, Func<object, object, Func<QueryContainerDescriptor<T>, QueryContainer>>>();
        private IDependencyResolver _resolver;

        public BoolQueryBulder(IDependencyResolver resolver)
        {
            this._resolver = resolver;
        }

        public Func<QueryContainerDescriptor<T>, QueryContainer> BuildQuery(QueryContext context)
        {
            var innerQueries = new List<Func<QueryContainerDescriptor<T>, QueryContainer>>();
            context.SearchFields.Aggregate(innerQueries, (col, next) =>
            {
                var clauseBuilderType = typeof(IQueryClauseBuilder<>)
                .MakeGenericType(next.GetType());
                var clauseBuilder = this._resolver.Resolve(clauseBuilderType);

                var clauseBuilderDelegate = this.GetClauseBuilderDelegate(next.GetType(), clauseBuilderType);
                var query = clauseBuilderDelegate(clauseBuilder, next);
                col.Add(query);
                return col;
            });

            Func<IEnumerable<Func<QueryContainerDescriptor<T>, QueryContainer>>, Func<QueryContainerDescriptor<T>, QueryContainer>> func = queryFunc =>
            {
                Func<QueryContainerDescriptor<T>, QueryContainer > fr = descriptor =>
                 {
                     var r = descriptor.Bool(b => b.Should(queryFunc));
 
                     return r;
                 };
                return fr;
            };

            return func(innerQueries);
        }

        protected Func<object, object, Func<QueryContainerDescriptor<T>, QueryContainer>> GetClauseBuilderDelegate(Type contextType, Type clauseBuilderType)
        {
            
            var del = this._delegateCache.GetOrAdd(contextType, k =>
            {
                var minfo = clauseBuilderType.GetMethod("BuildQueryClause")
                .MakeGenericMethod(typeof(T));
                var targetPar = Expression.Parameter(typeof(object));
                var contextPar = Expression.Parameter(typeof(object));
                var callExpression = Expression.Call(Expression.Convert(targetPar, clauseBuilderType), minfo, Expression.Convert(contextPar, contextType));
                var lambda = Expression.Lambda<Func<object, object, Func<QueryContainerDescriptor<T>, QueryContainer>>>(callExpression, targetPar, contextPar).Compile();
                return lambda;
            });

            return del;
        }

        protected virtual Expression<Func<T, object>> BuildPropertyExpression(FieldBase fielsContext)
        {
            //ToDo: Cache delegates
            var par = Expression.Parameter(typeof(T));
            var property = ReflectionHelper.GetPathAsExpression(par, fielsContext.Path);
            var lambda = Expression.Lambda<Func<T, object>>(property, par);
            return lambda;
        }
    }
}