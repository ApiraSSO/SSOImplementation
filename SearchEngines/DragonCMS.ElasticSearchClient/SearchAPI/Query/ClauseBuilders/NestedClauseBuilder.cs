using System;
using Kernel.Initialisation;
using Nest;
using SearchEngine.Infrastructure.Query;

namespace ElasticSearchClient.SearchAPI.Query.ClauseBuilders
{
    internal class NestedClauseBuilder : QueryClauseBuilder<NestedFieldContext>, IAutoRegisterAsTransient
    {
        public override Func<QueryContainerDescriptor<T>, QueryContainer> BuildQueryClause<T>(NestedFieldContext fieldContext)
        {
            Func<QueryContainerDescriptor<T>, QueryContainer> query = d =>
            {
                var field = base.BuildPropertyExpression<T>(fieldContext);
                var container = d.Nested(n =>
                                            n.Path(field)
                                            .Query(q => q.Match(td =>
                                                                    td.Field(new Field(fieldContext.PropertyName))
                                                                    .Query(fieldContext.Value.ToString()))));
                return container;
            };

            return query;
        }
    }
}