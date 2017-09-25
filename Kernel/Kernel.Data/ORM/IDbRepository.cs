namespace Kernel.Data.ORM
{
	/// <summary>
	/// Non generic Data base repository
	/// </summary>
	public interface IDbRepository
	{
		IDbContext Context { get; }
	}
}