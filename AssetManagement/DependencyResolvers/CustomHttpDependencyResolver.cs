using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

namespace AssetManagement.DependencyResolvers
{
	public class CustomHttpDependencyResolver : IDependencyResolver
	{
		private readonly Kernel.DependancyResolver.IDependencyResolver resolver;

		/// <summary>
		/// Initializes a new instance of the <see cref="CustomHttpDependencyResolver"/> class.
		/// </summary>
		/// <param name="resolver">The resolver.</param>
		public CustomHttpDependencyResolver(Kernel.DependancyResolver.IDependencyResolver resolver)
		{
			this.resolver = resolver;
		}

		/// <summary>
		/// Starts a resolution scope.
		/// Has a child S3Id dependency resolver which is disposed after out of score(request)
		/// </summary>
		/// <returns>
		/// The dependency scope.
		/// </returns>
		public IDependencyScope BeginScope()
		{
			return new CustomHttpDependencyResolver(this.resolver.CreateChildContainer());
		}

		/// <summary>
		/// Retrieves a service from the scope.
		/// </summary>
		/// <param name="serviceType">The service to be retrieved.</param>
		/// <returns>
		/// The retrieved service.
		/// </returns>
		public object GetService(Type serviceType)
		{
			object resolved;
			var isReolved = this.resolver.TryResolve(serviceType, out resolved);
			//log for debuging purpose
			//if (!isReolved)
				//LoggerManager.WriteInformationToEventLog(String.Format("The resolution of serviceType: {0} has failed.", serviceType.FullName));

			return resolved;
		}

		/// <summary>
		/// Retrieves a collection of services from the scope.
		/// </summary>
		/// <param name="serviceType">The collection of services to be retrieved.</param>
		/// <returns>
		/// The retrieved collection of services.
		/// </returns>
		public IEnumerable<object> GetServices(Type serviceType)
		{
			try
			{
				return this.resolver.ResolveAll(serviceType);
			}
			catch (Exception)
			{
				return Enumerable.Empty<object>();
			}
		}

		public void Dispose()
		{
			this.resolver.Dispose();
		}
	}
}