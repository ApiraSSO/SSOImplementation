using System.Threading;
using System.Threading.Tasks;

namespace Kernel.Federation.Tenant
{
    public interface IDocumentRetriever
    {
        Task<string> GetDocumentAsync(string address, CancellationToken cancel);
    }
}