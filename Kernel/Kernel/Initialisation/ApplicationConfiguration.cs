using System;
using System.Collections.Generic;
using System.Linq;
using Kernel.DependancyResolver;

namespace Kernel.Initialisation
{
    /// <summary>
    /// Global configuration file.
    /// </summary>
    public class ApplicationConfiguration
	{
		#region static fields

		/// <summary>
		/// Singlton Instance
		/// </summary>
		public readonly static ApplicationConfiguration Instance = new ApplicationConfiguration();

		#endregion

		#region static methods

		/// <summary>
		/// Registers the dependancy resolver.
		/// </summary>
		/// <param name="resolverFactory">The resolver.</param>
		public static void RegisterDependencyResolver(Func<IDependencyResolver> resolverFactory)
		{
			var resolver = resolverFactory();
			resolver.RegisterInstance<IDependencyResolver>(resolver, Lifetime.Singleton);
			ApplicationConfiguration.Instance.dependencyResolver = resolver;
		}

		/// <summary>
		/// Registers the cache provider.
		/// </summary>
		/// <param name="provider">The provider.</param>
		public static void RegisterService<TService>(TService provider, Lifetime lifeTime)
		{
			ApplicationConfiguration.Instance.DependencyResolver.RegisterType<TService>(lifeTime);
		}

		/// <summary>
		/// Registers the server initialiser factory.
		/// </summary>
		/// <param name="serverInitialiserFactory">The server initialiser factory.</param>
		public static void RegisterServerInitialiserFactory(Func<IServerInitialiser> serverInitialiserFactory)
		{
			ApplicationConfiguration.Instance.ServerInitialiserFactory = serverInitialiserFactory;
		}

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="S3IDConfiguration"/> class.
		/// </summary>
		protected ApplicationConfiguration()
		{
			this.SetDefaults();
		}

		#endregion

		#region fields
		IDependencyResolver dependencyResolver;
		Func<IServerInitialiser> serverInitialiserFactory;
		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the factory dependancy resolver factory.
		/// </summary>
		/// <value>
		/// The factory dependancy resolver factory.
		/// </value>
		public IDependencyResolver DependencyResolver
		{
			get { return this.dependencyResolver; }
			private set
			{
				this.dependencyResolver = value;
			}
		}

		/// <summary>
		/// Gets the server initialiser factory.
		/// </summary>
		/// <value>
		/// The server initialiser factory.
		/// </value>
		public Func<IServerInitialiser> ServerInitialiserFactory
		{
			get { return this.serverInitialiserFactory; }
			private set
			{
				this.serverInitialiserFactory = value;
			}
		}

		#endregion

		#region methods

		/// <summary>
		/// Sets the defaults.
		/// </summary>
		private void SetDefaults()
		{
			this.dependencyResolver = new DefaultResolver();
		}

		#endregion

		#region nested clases
		/// <summary>
		/// Default dependancy resolver
		/// </summary>
		/// <seealso cref="S3ID.PAS3.Server.Infrastructure.DependancyResolver.IDependencyResolver" />
		private class DefaultResolver : IDependencyResolver
		{
			public bool TryResolve<T>(out T resolved)
			{
				throw new NotImplementedException();
			}

			public bool TryResolve(Type type, out object resolved)
			{
				throw new NotImplementedException();
			}

			/// <summary>
			/// Resolves this instance.
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <returns></returns>
			public T Resolve<T>()
			{
				return (T)this.Resolve(typeof(T));
			}

			/// <summary>
			/// Resolves the specified type.
			/// </summary>
			/// <param name="type">The type.</param>
			/// <returns></returns>
			public object Resolve(Type type)
			{
				if (type.IsAbstract || type.IsInterface)
					return null;
				return Activator.CreateInstance(type);
			}

			/// <summary>
			/// Registers the instance.
			/// </summary>
			/// <param name="t">The t.</param>
			/// <param name="name">The name.</param>
			/// <param name="instance">The instance.</param>
			/// <param name="lifetime">The lifetime.</param>
			/// <returns></returns>
			/// <exception cref="System.NotImplementedException"></exception>
			public IDependencyResolver RegisterInstance(Type t, string name, object instance, Lifetime lifetime)
			{
				throw new NotImplementedException();
			}

			/// <summary>
			/// Registers the instance.
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <param name="name">The name.</param>
			/// <param name="instance">The instance.</param>
			/// <param name="lifetime">The lifetime.</param>
			/// <returns></returns>
			/// <exception cref="System.NotImplementedException"></exception>
			public IDependencyResolver RegisterInstance<T>(string name, T instance, Lifetime lifetime)
			{
				throw new NotImplementedException();
			}

			/// <summary>
			/// Registers the specified name.
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <param name="name">The name.</param>
			/// <param name="factory">The factory.</param>
			/// <param name="lifetime">The lifetime.</param>
			/// <returns></returns>
			/// <exception cref="System.NotImplementedException"></exception>
			public IDependencyResolver RegisterFactory<T>(Func<T> factory, Lifetime lifetime)
			{
				throw new NotImplementedException();
			}

			public object Container
			{
				get { throw new NotImplementedException(); }
			}

			/// <summary>
			/// Registers the type.
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <param name="name">The name.</param>
			/// <param name="type">The type.</param>
			/// <param name="lifetime">The lifetime.</param>
			/// <returns></returns>
			/// <exception cref="System.NotImplementedException"></exception>
			public IDependencyResolver RegisterType<T>(Lifetime lifetime)
			{
				throw new NotImplementedException();
			}

			public IDependencyResolver RegisterType(Type type, Lifetime lifetime)
			{
				throw new NotImplementedException();
			}

			public IEnumerable<T> ResolveAll<T>()
			{
				return this.ResolveAll(typeof(T)).Cast<T>();
			}

			public IEnumerable<object> ResolveAll(Type type)
			{
				return Enumerable.Empty<object>();
			}

			public IDependencyResolver RegisterInstance<T>(T instance, Lifetime lifetime)
			{
				throw new NotImplementedException();
			}


			public bool Contains<T>()
			{
				return false;
			}

			public bool Contains(Type type)
			{
				throw new NotImplementedException();
			}


			public IDependencyResolver CreateChildContainer()
			{
				throw new NotImplementedException();
			}


			public IDependencyResolver RegisterFactory<T>(Func<Type, T> factory, Lifetime lifetime)
			{
				throw new NotImplementedException();
			}


			public IDependencyResolver RegisterFactory(Type type, Func<Type, object> factory, Lifetime lifetime)
			{
				throw new NotImplementedException();
			}

			public void Dispose()
			{
				throw new NotImplementedException();
			}


			public IDependencyResolver RegisterNamedType(Type type, string name, Lifetime lifetime)
			{
				throw new NotImplementedException();
			}
		}

		#endregion
	}
}