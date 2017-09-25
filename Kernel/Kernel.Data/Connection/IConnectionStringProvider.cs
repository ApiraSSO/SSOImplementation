using System.Data.SqlClient;
namespace Kernel.Data.Connection
{
	/// <summary>
	///     Interface IConnectionStringProvider. Builds connection string for database provider
	/// </summary>
	public interface IConnectionStringProvider
	{
		/// <summary>
		///     Gets the connection string.
		/// </summary>
		/// <returns>The connection string</returns>
		SqlConnectionStringBuilder GetConnectionString();
	}
}