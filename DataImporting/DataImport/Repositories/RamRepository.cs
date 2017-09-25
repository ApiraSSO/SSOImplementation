using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Importing.Infrastructure;
using Kernel.Data;
using Kernel.Data.DataRepository;

namespace Data.Importing.Repositories
{
    internal class RamRepository : IReadOnlyRepository<ImportedEntry, Guid>
    {
        private ICollection<ImportedEntry> _entries;
        //private IEnumerable<ImportedEntry> enumerable;

        public RamRepository() : this(Enumerable.Empty<ImportedEntry>())
        {
        }

        public RamRepository(IEnumerable<ImportedEntry> enumerable)
        {
            this._entries = enumerable
                .ToList();
        }

        public IQueryable<ImportedEntry> Read()
        {
            return this._entries.AsQueryable();
        }

        public Task<IHasID<Guid>> Read(Guid ID)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ImportedEntry> ReadBatch(IQueryable<ImportedEntry> query, int offset, int batchSize)
        {
            return this._entries.AsQueryable()
                .OrderBy(x => x.Id)
                .Skip(offset)
                .Take(batchSize);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            this._entries.Clear();
        }
    }
}