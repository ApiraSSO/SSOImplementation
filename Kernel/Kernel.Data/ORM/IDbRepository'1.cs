namespace Kernel.Data.ORM
{
	using System;
	using Kernel.Data;
	using Kernel.Data.DataRepository;

	/// <summary>
	///  Database repository
	/// </summary>
	public interface IDbRepository<T> : IDbRepository, IRepository<T, Guid>
		where T : class, IHasID<Guid>
	{
	}
}