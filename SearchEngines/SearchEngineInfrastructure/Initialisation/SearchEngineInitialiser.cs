using System.Threading.Tasks;
using Kernel.DependancyResolver;
using Shared.Initialisation;

namespace SearchEngine.Initialisation
{
    public class SearchEngineInitialiser : Initialiser
    {
        public override byte Order
        {
            get
            {
                return 0;
            }
        }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            //Register default document dispatcher. It could be overwritten by ApplicationConfiguration.RegisterDocumentDispatcherFactory
            this.RegisterDispatcherDefault(dependencyResolver);
            return Task.CompletedTask;
        }

        private void RegisterDispatcherDefault(IDependencyResolver dependencyResolver)
        {
            //default dispatcher. It could be loaded from config or any source.
            //supported dispatcher elastic search client(DocumentDispatcher in DragonCMS.ElasticSearchClient.DocumentAPI) 
            //and kafka client (KafkaDispatcher in DragonCMS.KafkaClient.ProducerClient)
            //ApplicationConfiguration.RegisterDocumentDispatcherFactory(() => dependencyResolver.Resolve<DocumentDispatcher>());
            //ApplicationConfiguration.RegisterDocumentDispatcherFactory(() => dependencyResolver.Resolve<KafkaDispatcherMS>());
            //dependencyResolver.RegisterFactory<IDocumentDispatcher>(() =>
            //{
            //    return ApplicationConfiguration.Instance.DocumentDispatcherFactory();
            //}, Lifetime.Singleton);
        }
    }
}