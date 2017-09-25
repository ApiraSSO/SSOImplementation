namespace Kernel.DependancyResolver
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// DependencyResolver members
	/// </summary>
	public interface IDependencyResolver : IDisposable
	{
		/// <summary>
		/// Resolves this instance.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		T Resolve<T>();
		/// <summary>
		/// Resolves the specified type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		object Resolve(Type type);

		/// <summary>
		/// Tries the resolve.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="resolved">The resolved.</param>
		/// <returns></returns>
		bool TryResolve<T>(out T resolved);

		/// <summary>
		/// Tries the resolve.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="resolved">The resolved.</param>
		/// <returns></returns>
		bool TryResolve(Type type, out object resolved);

		/// <summary>
		/// Resolves all.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		IEnumerable<T> ResolveAll<T>();
		/// <summary>
		/// Resolves all.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		IEnumerable<object> ResolveAll(Type type);
		/// <summary>
		/// Registers the instance.
		/// </summary>
		/// <param name="t">The t.</param>
		/// <param name="name">The name.</param>
		/// <param name="instance">The instance.</param>
		/// <param name="lifetime">The lifetime.</param>
		/// <returns></returns>
		IDependencyResolver RegisterInstance(Type t, string name, object instance, Lifetime lifetime);
		/// <summary>
		/// Registers the instance.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name">The name.</param>
		/// <param name="instance">The instance.</param>
		/// <param name="lifetime">The lifetime.</param>
		/// <returns></returns>
		IDependencyResolver RegisterInstance<T>(string name, T instance, Lifetime lifetime);
		/// <summary>
		/// Registers the instance.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instance">The instance.</param>
		/// <param name="lifetime">The lifetime.</param>
		/// <returns></returns>
		IDependencyResolver RegisterInstance<T>(T instance, Lifetime lifetime);
		/// <summary>
		/// Registers the type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="lifetime">The lifetime.</param>
		/// <returns></returns>
		IDependencyResolver RegisterType<T>(Lifetime lifetime);
		/// <summary>
		/// Registers the type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="lifetime">The lifetime.</param>
		/// <returns></returns>
		IDependencyResolver RegisterType(Type type, Lifetime lifetime);

		/// <summary>
		/// Registers the type of the named.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="lifetime">The lifetime.</param>
		/// <returns></returns>
		IDependencyResolver RegisterNamedType(Type type, string name, Lifetime lifetime);

		/// <summary>
		/// Registers the specified factory.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="factory">The factory.</param>
		/// <param name="lifetime">The lifetime.</param>
		/// <returns></returns>
		IDependencyResolver RegisterFactory<T>(Func<T> factory, Lifetime lifetime);
		/// <summary>
		/// Registers the specified factory.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="factory">The factory.</param>
		/// <param name="lifetime">The lifetime.</param>
		/// <returns></returns>
		IDependencyResolver RegisterFactory<T>(Func<Type, T> factory, Lifetime lifetime);
		/// <summary>
		/// Registers the specified type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="factory">The factory.</param>
		/// <param name="lifetime">The lifetime.</param>
		/// <returns></returns>
		IDependencyResolver RegisterFactory(Type type, Func<Type, object> factory, Lifetime lifetime);
		/// <summary>
		/// Determines whether [contains].
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		bool Contains<T>();
		/// <summary>
		/// Determines whether [contains] [the specified type].
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		bool Contains(Type type);
		/// <summary>
		/// Creates the child container.
		/// </summary>
		/// <returns></returns>
		IDependencyResolver CreateChildContainer();
	}
}
