using System.Threading;
using System.Threading.Tasks;

namespace Kernel.Federation.RelyingParty
{
    public interface IDocumentRetriever
    {
        Task<string> GetDocumentAsync(string address, CancellationToken cancel);
    }
}