using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ElasticSearchClient.ErrorHandling;
using ElasticSearchClient.Factories;
using Kernel.DependancyResolver;
using Nest;
using SearchEngine.Infrastructure;
using SearchEngine.Infrastructure.Query;

namespace ElasticSearchClient.IndexAPI
{
    internal class IndexManager : IIndexManager
    {
        ConcurrentDictionary<string, IndexState> _indices = new ConcurrentDictionary<string, IndexState>();
        ConcurrentDictionary<string, IDictionary<string, IProperty>> _propertiesMapping = new ConcurrentDictionary<string, IDictionary<string, IProperty>>();

        private readonly ReaderWriterLockSlim _readerWriterLockSlim = new ReaderWriterLockSlim();

        private readonly IDependencyResolver _resolver;
        private readonly IClientFactory _clientFactory;
        private readonly IResponseHandler _responseHandler;
        private readonly ITypeToIndexMapperManager _typeToIndexMapperManager;

        public IndexManager(IDependencyResolver resolver, IClientFactory clientFactory, IResponseHandler responseHandler)
            :this(resolver, clientFactory, responseHandler, new indexMapperDefault())
        {
        }
        public IndexManager(IDependencyResolver resolver, IClientFactory clientFactory, IResponseHandler responseHandler, ITypeToIndexMapperManager typeToIndexMapperManager)
        {
            this._resolver = resolver;
            this._clientFactory = clientFactory;
            this._responseHandler = responseHandler;
            this._typeToIndexMapperManager = typeToIndexMapperManager;
        }

        public async Task<IndexName> GetIndex(IndexContext indexContext)
        {
            var client = this._clientFactory.GetClient();
            
            var index = this.BuildIndexName(indexContext);
            await this.BuildIndex(index);
            return index;
        }

        Task IIndexManager.BuildAllIndices()
        {
            throw new NotImplementedException();
        }

        public Task BuildIndex(IndexName index)
        {
            var client = this._clientFactory.GetClient();
            try
            {
                this._readerWriterLockSlim.EnterWriteLock();

                var indexExists = client.IndexExists(index);

                if (!indexExists.Exists)
                    this.CreateIndex(index, client);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this._readerWriterLockSlim.ExitWriteLock();
            }
            
            return Task.CompletedTask;
        }

        public Task DeleteIndex(Type type, string indexName = null)
        {
            var indexContext = new IndexContext(type, indexName);
            return this.DeleteIndex(indexContext);
        }

        public Task DeleteIndex(IndexContext indexContext)
        {
            var client = this._clientFactory.GetClient();
            var index = this.BuildIndexName(indexContext);
            var response = client.DeleteIndex(index);
            this._responseHandler.ValdateAndHandleException(response, true);
            return Task.CompletedTask;
        }
       
        public IndexName BuildIndexName(IndexContext indexContext)
        {
            return new IndexName { Name = indexContext.IndexName, Type = indexContext.IndexType };
        }

        public IProperty GetPropertyMetaData<TIndex>(IndexContext<TIndex> indexContext, Expression<Func<TIndex, object>> property)
        {
            throw new NotImplementedException();
        }

        public IProperty GetPropertyMetaData<TIndex>(IndexContext<TIndex> indexContext, string propertyPath)
        {
            var properties = this._propertiesMapping.GetOrAdd(indexContext.IndexName, k =>
            {
                var indexState = this.GetIndexState(indexContext);
                var propertiesMapped = indexState
                       .Mappings.SelectMany(x => x.Value.Properties);

                var projection = this.ProjectProperties(typeof(TIndex).Name, propertiesMapped);
                return projection;
            });
            
            return properties.FirstOrDefault(x => x.Key.Equals(propertyPath, StringComparison.OrdinalIgnoreCase)).Value;
        }

        public IndexState GetIndexState(IndexContext indexContext)
        {
            var index = this.BuildIndexName(indexContext);
            return this._indices.GetOrAdd(index.Name, k =>
            {
                var client = this._clientFactory.GetClient();
                var response = client.GetIndex(index);
                this._responseHandler.ValdateAndHandleException(response, true);
                var indexState = response.Indices.Single();
                return indexState.Value;
            });
        }

        public ITypeToIndexMapper GetMapper(Type type)
        {
            return this._typeToIndexMapperManager.GetMapper(type);
        }

        private IDictionary<string, IProperty> ProjectProperties(string root, IEnumerable<KeyValuePair<PropertyName, IProperty>> properties)
        {
            var path = new StringBuilder();
            path.Append(root);

            var result = new Dictionary<string, IProperty>();
            this.RecursivelyProject(root, properties, result);
            
            return result;
        }

        private void RecursivelyProject(string root, IEnumerable<KeyValuePair<PropertyName, IProperty>> properties, IDictionary<string, IProperty> result)
        {
            var path = new StringBuilder();
            path.Append(root);

            foreach (var p in properties)
            {
                result.Add(String.Format("{0}.{1}", root, this.Capitalise(p.Key.Name)), p.Value);
                var objectProperty = p.Value as IObjectProperty;
                if (objectProperty == null || objectProperty.Properties == null)
                {
                    continue;
                }
                var capitalised = this.Capitalise(p.Key.Name);
                
                var newRoot = String.Format("{0}.{1}", root, this.Capitalise(p.Key.Name));
                this.RecursivelyProject(newRoot, objectProperty.Properties, result);
            }
        }

        private string Capitalise(string s)
        {
            var firstChar = char.ToUpper(s[0]).ToString();
            return s.Remove(0, 1)
                .Insert(0, firstChar);
        }
        
        private void CreateIndex(IndexName index, ElasticClient client)
        {
            var descriptor = new CreateIndexDescriptor(index);
           
            var mappers = this.ResolveMappers(index.Type);
            mappers.Aggregate(descriptor, (d, next) => next.Map(d) );

            var response = client.CreateIndex(descriptor);

            this._responseHandler.ValdateAndHandleException(response, true);
        }

        private IEnumerable<IIndexMapper> ResolveMappers(Type type)
        {
            var typeToResolve = typeof(IndexMapper<>).MakeGenericType(type);
            var mappers = this._resolver.ResolveAll(typeToResolve).Cast<IIndexMapper>();
            return mappers;
        }

        private class indexMapperDefault : ITypeToIndexMapperManager
        {
            public ITypeToIndexMapper GetMapper(Type type)
            {
                throw new NotImplementedException();
            }

            public ITypeToIndexMapperManager RegisterMapper(Type type, ITypeToIndexMapper mapper)
            {
                throw new NotImplementedException();
            }
        }
    }
}