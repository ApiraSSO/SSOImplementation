using System;
using Kernel.Initialisation;
using Nest;
using SearchEngine.Infrastructure.Query;

namespace ElasticSearchClient.SearchAPI.Query.ClauseBuilders
{
    internal class TermClauseBuilder : QueryClauseBuilder<FieldDescriptor>, IAutoRegisterAsTransient
    {
        //ToDo: not clear which one yet. TBC 2017/03/13
        public override Func<QueryContainerDescriptor<T>, QueryContainer> BuildQueryClause<T>(FieldDescriptor fieldContext)
        {
            Func<QueryContainerDescriptor<T>, QueryContainer> query = d =>
            {
                var field = base.BuildPropertyExpression<T>(fieldContext);
                var container = d.Term(td => td.Field(field)
                .Value(fieldContext.Value));
                return container;
            };

            return query;
        }

        //public override Func<QueryContainerDescriptor<T>, QueryContainer> BuildQueryClause<T>(FieldContext fieldContext)
        //{
        //    Func<QueryContainerDescriptor<T>, QueryContainer> query = d =>
        //    {
        //        var field = base.BuildPropertyExpression<T>(fieldContext);
        //        var container = d.Match(td => td.Field(field)
        //        .Query(fieldContext.Value.ToString()));
        //        return container;
        //    };

        //    return query;
        //}
    }
}