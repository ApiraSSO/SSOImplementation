using System.Threading.Tasks;
using Kernel.DependancyResolver;
using Provider.EntityFramework.Resolvers;
using Shared.Initialisation;

namespace Provider.EntityFramework.Initialisation
{
    public class DbContextInitialiser : Initialiser
    {
        public override byte Order
        {
            get { return 0; }
        }

        /// <summary>
        /// Performs initialisation.
        /// </summary>
        /// <param name="dependencyResolver"></param>
        /// <returns></returns>
        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            //Register EF context manually. Multiple context may exist to pick from
            dependencyResolver.RegisterType<DBContext>(Lifetime.Transient);
            //Register connection string dependency in ConnectionDefinitionParser(..., Func<PropertyInfo, string> configNameConverter)
            //dependencyResolver.RegisterFactory<Func<PropertyInfo, string>>(() => x => x.Name, Lifetime.Transient);
            ////Register connection string dependency in ConnectionDefinitionParser(Func<NameValueCollection> connectionPropertiesFactory, ...)
            //dependencyResolver.RegisterFactory<Func<NameValueCollection>>(() => () => ConfigurationManager.AppSettings, Lifetime.Transient);
            //Register type provider manually as an singleton
            dependencyResolver.RegisterType<TypeProviderService>(Lifetime.Singleton);
            return Task.FromResult<object>(null);
        }
    }
}
