using System.Threading;
using System.Threading.Tasks;

namespace Kernel.Federation.RelyingParty
{
    public interface IConfigurationRetriever<T>
    {
        Task<T> GetAsync(string address, CancellationToken cancel);
    }
}