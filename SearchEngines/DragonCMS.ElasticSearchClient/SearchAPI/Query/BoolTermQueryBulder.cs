using System;
using DragonCMS.Common.Dependencies;
using DragonCMS.Common.SearchEngine.Query;
using Nest;

namespace DragonCMS.ElasticSearchClient.SearchAPI.Query
{
    internal class BoolTermQueryBulder<T> : BoolQueryBulder<T> where T : class
    {
        public BoolTermQueryBulder(IDependencyResolver resolver) : base(resolver)
        {
        }

        protected override Func<QueryContainerDescriptor<T>, QueryContainer> BuildNestedQueryInternal(NestedFieldContext fieldContext)
        {
            Func<QueryContainerDescriptor<T>, QueryContainer> query = d =>
            {
                var field = base.BuildPropertyExpression(fieldContext);
                var container = d.Nested(n => 
                                            n.Path(field)
                                            .Query(q => q.Match(td => 
                                                                    td.Field(new Field(fieldContext.PropertyName))
                                                                    .Query(fieldContext.Value.ToString()))));
                return container;
            };

            return query;
        }

        protected override Func<QueryContainerDescriptor<T>, QueryContainer> BuildQueryInternal(FieldContext fieldContext)
        {
            Func<QueryContainerDescriptor<T>, QueryContainer> query = d =>
            {
                var field = base.BuildPropertyExpression(fieldContext);
                var container = d.Term(td => td.Field(field)
                .Value(fieldContext.Value));
                return container;
            };

            return query;
        }
    }
}