using System.IO;
using System.Threading.Tasks;
using Kernel.Authentication;

namespace Data.Importing.Infrastructure.Contexts
{
    public abstract class ResourceContext
    {
        protected readonly ICredentialsProvider CredentialsProvider;

        public ResourceContext(ICredentialsProvider credentialsProvider)
        {
            this.CredentialsProvider = credentialsProvider;
        }

        public Task<Stream> GetSourceStream()
        {
            return this.GetStreamInternal();
        }

        protected abstract Task<Stream> GetStreamInternal();
    }
}