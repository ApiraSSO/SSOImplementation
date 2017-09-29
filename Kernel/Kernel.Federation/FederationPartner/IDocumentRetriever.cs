using System.Threading;
using System.Threading.Tasks;

namespace Kernel.Federation.FederationPartner
{
    public interface IDocumentRetriever
    {
        Task<string> GetDocumentAsync(string address, CancellationToken cancel);
    }
}