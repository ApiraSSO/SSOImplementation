using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using AssetManagement.DependencyResolvers;
using Kernel.Initialisation;
using Kernel.Logging;
using ServerInitialisation;
using UnityResolver;

namespace AssetManagement
{
    public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			using (new InformationLogEventWriter())
			{
				ApplicationConfiguration.RegisterDependencyResolver(() => new UnityDependencyResolver());
				ApplicationConfiguration.RegisterServerInitialiserFactory(() => new ServerInitialiser());

				this.RegisterWebConfiguration();
				this.InitializeServer();
			}

			//AreaRegistration.RegisterAllAreas();
			//GlobalConfiguration.Configure(WebApiConfig.Register);
			//FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			//RouteConfig.RegisterRoutes(RouteTable.Routes);
			//BundleConfig.RegisterBundles(BundleTable.Bundles);
		}

		/// <summary>
		/// Registers the web configuration.
		/// NOTE: Keep the order in the method
		/// </summary>
		private void RegisterWebConfiguration()
		{
			//register the areas first
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			//register WebApi routes first
			GlobalConfiguration.Configure(WebApiConfig.Register);
			//register mvc routes
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			//WebApi dependency resolver
			GlobalConfiguration.Configuration.DependencyResolver = new CustomHttpDependencyResolver(ApplicationConfiguration.Instance.DependencyResolver);
		}

		/// <summary>
		/// Initializes the server.
		/// </summary>
		private void InitializeServer()
		{
			using (new InformationLogEventWriter())
			{
				var container = ApplicationConfiguration.Instance.DependencyResolver;
				var initialiser = ApplicationConfiguration.Instance.ServerInitialiserFactory();
				var task = initialiser.Initialise(container);
			}
		}
	}
}