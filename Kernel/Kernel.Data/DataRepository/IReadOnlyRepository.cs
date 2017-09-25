using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kernel.Data.DataRepository
{
	public interface IReadOnlyRepository<T, TID> : IRepository<TID>
		where T : class, IHasID<TID>
		where TID : struct
	{
        IQueryable<T> Read();

        IQueryable<T> ReadBatch(IQueryable<T> query, int offset, int batchSize);
    }
}