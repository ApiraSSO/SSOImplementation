using System;
using System.Threading.Tasks;

namespace Kernel.Data.DataRepository
{
	public interface IRepository<TID> : IDisposable
        where TID : struct
	{
		/// <summary>
		///     Reads a single item with the given ID.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		Task<IHasID<TID>> Read(TID ID);
	}
}
