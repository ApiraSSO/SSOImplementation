using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Kernel.Initialisation;
using Nest;
using SearchEngine.Infrastructure;
using SearchEngine.Infrastructure.Query;

namespace ElasticSearchClient.IndexAPI
{
    public interface IIndexManager : IAutoRegisterAsTransient
    {
        Task<IndexName> GetIndex(IndexContext indexContext);
        Task BuildAllIndices();

        Task DeleteIndex(Type type, string indexName = null);
        Task DeleteIndex(IndexContext indexContext);

        IndexName BuildIndexName(IndexContext indexContext);

        IndexState GetIndexState(IndexContext indexContext);

        IProperty GetPropertyMetaData<TIndex>(IndexContext<TIndex> indexContext, Expression<Func<TIndex, object>> property);
        
        ITypeToIndexMapper GetMapper(Type type);

        IProperty GetPropertyMetaData<TIndex>(IndexContext<TIndex> indexContext, string propertyPath);
    }
}