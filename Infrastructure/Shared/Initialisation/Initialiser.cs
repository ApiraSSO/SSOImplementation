namespace Shared.Initialisation
{
	using System.Threading.Tasks;
	using Kernel.DependancyResolver;

	/// <summary>
	/// Base class for all Ininialisers
	/// </summary>
	public abstract class Initialiser
	{
		/// <summary>
		/// Gets the order the initialiser should run in.
		/// </summary>
		/// <value>
		/// The order.
		/// </value>
		public abstract byte Order { get; }

		/// <summary>
		/// Performs initialisation.
		/// </summary>
		/// <param name="container"></param>
		/// <returns></returns>
		public async Task Initialise(IDependencyResolver dependencyResolver)
		{
			await this.InitialiseInternal(dependencyResolver);
		}

		/// <summary>
		/// Performs initialisation.
		/// </summary>
		/// <param name="container"></param>
		/// <returns></returns>
		protected abstract Task InitialiseInternal(IDependencyResolver dependencyResolver);
	}
}
