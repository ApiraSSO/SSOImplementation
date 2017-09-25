using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Kernel.Initialisation;
using Kernel.Reflection;
using Nest;
using Infrastructure = SearchEngine.Infrastructure.Query;

namespace ElasticSearchClient.SearchAPI
{
    internal class SortClauseBuilder<T> : IAutoRegisterAsTransient where T : class
    {
        internal Func<SortDescriptor<T>, IPromise<IList<ISort>>> BuildSortClause(Infrastructure.SortContext context) 
        {
            return des =>
            {
                foreach (var f in context.Fields)
                {
                    var fieldExp = this.BuildPropertyExpression(f);
                    des.Field(fieldExp, f.SortOrder == Infrastructure.SortOrder.Ascending ? Nest.SortOrder.Ascending : Nest.SortOrder.Descending);
                }

                return des;
            };
        }

        protected virtual Expression<Func<T, object>> BuildPropertyExpression(Infrastructure.FieldBase fielsContext)
        {
            //!!!!!!!!ToDo: sort this out. code repetition. BAD BAD BAD, TERRIBLE, IDIOTIC!!!!!!!!!!!!
            var par = Expression.Parameter(typeof(T));
            var property = ReflectionHelper.GetPathAsExpression(par, fielsContext.Path);
            var lambda = Expression.Lambda<Func<T, object>>(property, par);
            return lambda;
        }
    }
}