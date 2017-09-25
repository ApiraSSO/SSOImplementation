using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ElasticSearchClient.ErrorHandling;
using ElasticSearchClient.Factories;
using ElasticSearchClient.SearchAPI.ResultProjectors;
using Kernel.DependancyResolver;
using Kernel.Initialisation;
using SearchEngine.Infrastructure;
using SearchEngine.Infrastructure.Query;

namespace ElasticSearchClient.SearchAPI
{
    internal class SearchEngine : ISearchEngine, IAutoRegisterAsTransient
    {
        private static ConcurrentDictionary<Type, Func<ISearchEngine, QueryContext, Task<SearchResult<QmSearchResult>>>> _delegateCache = new ConcurrentDictionary<Type, Func<ISearchEngine, QueryContext, Task<SearchResult<QmSearchResult>>>>();
        private readonly IClientFactory _clientFactory;
        private readonly IDependencyResolver _dependencyResolver;
        private readonly IResponseHandler _responseHandler;
        
        public SearchEngine(IClientFactory clientFactory, IDependencyResolver dependencyResolver, IResponseHandler responseHandler)
        {
            this._clientFactory = clientFactory;
            this._dependencyResolver = dependencyResolver;
            this._responseHandler = responseHandler;
        }
        public TProjection SearchById<TAggregate, TProjection>(Guid id)
            where TAggregate : class
            where TProjection : class
        {
            throw new NotImplementedException();
        }

        public Task<SearchResult<QmSearchResult>> Search(QueryContext context)
        {
            var del = SearchEngine._delegateCache.GetOrAdd(context.IndexContext.IndexType, k => SearchEngine.BuildDel(k));
            return del(this, context);
        }
        public Task<SearchResult<TResult>> Search<TModel, TResult>(QueryContext context)
            where TModel : class
            where TResult : class
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if(context.IndexContext == null)
                throw new ArgumentNullException("indexContext");

            var clauseBuilder = this._dependencyResolver.Resolve<ISearchClauseBuilder<TModel>>();
            var clause = clauseBuilder.BuildSearchClause(context);
          
            var client = this._clientFactory.GetClient();
            var searchResponse = clause.Compile()(client);
            this._responseHandler.ValdateAndHandleException(searchResponse, true);
            var resultProjector = this._dependencyResolver.Resolve<IResultProjector<TModel, TResult>>();
            var result = resultProjector.GetResult(searchResponse);
            return Task.FromResult(result);
        }

        private static Func<ISearchEngine, QueryContext, Task<SearchResult<QmSearchResult>>> BuildDel(Type type)
        {
            var method = typeof(ISearchEngine).GetMethods()
                .Where(x => x.Name == "Search" && x.IsGenericMethod)
                .Single()
                .MakeGenericMethod(new[] { type, typeof(QmSearchResult) });
            var par = Expression.Parameter(typeof(QueryContext));
            var parTarget = Expression.Parameter(typeof(ISearchEngine));

            var call = Expression.Call(parTarget, method, par);
            var lambda = Expression.Lambda<Func<ISearchEngine, QueryContext, Task<SearchResult<QmSearchResult>>>>(call, parTarget, par)
                .Compile();
            return lambda;
        }
    }
}