using System;
using System.Linq.Expressions;
using Kernel.Reflection;
using Nest;
using SearchEngine.Infrastructure.Query;

namespace ElasticSearchClient.SearchAPI.Query.ClauseBuilders
{
    internal abstract class QueryClauseBuilder<TField> : IQueryClauseBuilder<TField> where TField  : FieldBase
    {
        public abstract Func<QueryContainerDescriptor<T>, QueryContainer> BuildQueryClause<T>(TField fieldContext) where T : class;

        protected virtual Expression<Func<T, object>> BuildPropertyExpression<T>(FieldBase fielsContext)
        {
            var par = Expression.Parameter(typeof(T));
            var property = ReflectionHelper.GetPathAsExpression(par, fielsContext.Path);
            var lambda = Expression.Lambda<Func<T, object>>(property, par);
            return lambda;
        }
    }
}