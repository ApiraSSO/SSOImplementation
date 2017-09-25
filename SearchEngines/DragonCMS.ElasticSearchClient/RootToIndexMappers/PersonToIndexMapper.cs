using System;
using System.Collections.Generic;

namespace ElasticSearchClient.RootToIndexMappers
{
    internal class PersonToIndexMapper : ITypeToIndexMapper<Person, EsPersonSearch>
    {
        public void BuildIndexContext(QueryContext queryContext)
        {
            if (queryContext == null)
                throw new ArgumentNullException("queryContext");

            queryContext.IndexContext = new IndexContext(typeof(EsPersonSearch));
        }

        public QueryContext BuildQueryContext(PagedSearchRequest searchRequest)
        {
            var context = new QueryContext();
            this.BuildIndexContext(context);
            context.SearchFields = new[]
            {
                new FieldContext { Path = "PersonName.LastName", Value = searchRequest.Text },
                new FieldContext { Path = "PersonName.FirstName", Value = searchRequest.Text }
            };
            context.SortContext.Fields.Add(new SortField { Path = "PersonName.FirstName" });
            return context;
        }

        public IEnumerable<FieldContext> GetFieldsContext()
        {
            throw new NotImplementedException();
        }

        public Type GetMappedIndexType()
        {
            return typeof(EsPersonSearch);
        }

        public void RegisterMapper(ITypeToIndexMapperManager typeToIndexMapperManager)
        {
            typeToIndexMapperManager.RegisterMapper(typeof(Person), this);
        }
    }
}