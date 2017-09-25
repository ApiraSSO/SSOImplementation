using System.Collections.Generic;

namespace Kernel.Providers
{
	public interface IIdentitytProvider<T> where T : struct
	{
		/// <summary>
		///     Gets an ID.
		/// </summary>
		/// <returns>T.</returns>
		T GetId();

		/// <summary>
		///     Gets the IDs in a batch with size passed as a parameter. Useful if IDs need to be created in transaction
		/// </summary>
		/// <param name="batchSize">Size of the batch.</param>
		/// <returns>IEnumerable&lt;T&gt;.</returns>
		IEnumerable<T> GetIdBatch(int batchSize);
	}
}