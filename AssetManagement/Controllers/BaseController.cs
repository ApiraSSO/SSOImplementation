using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using CQRS.Infrastructure.Messaging;
using Kernel.CQRS.Dispatching;
using Kernel.DependancyResolver;
using Shared.Services.Command;

namespace AssetManagement.Controllers
{
	/// <summary>
	/// Base controller
	/// </summary>
	/// <typeparam name="TService"></typeparam>
	public abstract class BaseController<TService> : ApiController
	{
		/// <summary>
		/// Service
		/// </summary>
		protected readonly TService Service;

		/// <summary>
		/// The dependency resolver
		/// </summary>
		public IDependencyResolver DependencyResolver { protected get; set; }

		/// <summary>
		/// Gets the resolve command service.
		/// </summary>
		/// <value>
		/// The resolve command service.
		/// </value>
		protected IMessageDispatcher<Command> CommandDispacher
		{
			get
			{
				if (this.DependencyResolver == null)
					throw new InvalidOperationException("Dependency resolver not register in DI container");

				return this.DependencyResolver.Resolve<IMessageDispatcher<Command>>();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseController{TService}"/> class.
		/// </summary>
		/// <param name="service">The service.</param>
		/// <param name="dependencyResolver">The dependency resolver.</param>
		protected BaseController(TService service)
		{
			this.Service = service;
		}

		/// <summary>
		/// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				var disposible = this.Service as IDisposable;
				if (disposible != null)
					disposible.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}