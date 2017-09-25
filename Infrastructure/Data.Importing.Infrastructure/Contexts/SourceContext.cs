using System;
using System.Threading.Tasks;
using Kernel.Data.DataRepository;

namespace Data.Importing.Infrastructure.Contexts
{
    public class SourceContext
    {
        private Lazy<Task<StageResultContext>> _source;
        public Task<StageResultContext> Source
        {
            get
            {
                return this._source.Value;
            }
        }

        public SourceContext(Func<Task<StageResultContext>> source)
        {
            this._source = new Lazy<Task<StageResultContext>>(source);
        }
    }
}