using System.Threading.Tasks;

namespace Kernel.Data.DataRepository
{
    public interface IRepository<T, TID> : IReadOnlyRepository<T, TID>
		where T : class, IHasID<TID>
		where TID : struct
	{
		/// <summary>
		///     Creates the specified item.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="item">The item.</param>
		/// <returns>The ID of the newly created item, or null if creation was not successful</returns>
		Task<TID?> Create(T item);

		/// <summary>
		///     Updates the specified item.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		Task<bool> Update(T item);

		/// <summary>
		///     Deletes the specified item.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		Task<bool> Delete(T item);

		/// <summary>
		///     Deletes the specified item.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="ID">The identifier.</param>
		/// <returns></returns>
		Task<bool> Delete(TID ID);
	}
}