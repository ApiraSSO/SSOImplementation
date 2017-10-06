using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kernel.Federation.FederationPartner
{
    public interface IConfigurationRetriever<T>
    {
        Task<T> GetAsync(FederationPartyContext context, CancellationToken cancel);
        Action<T> MetadataReceivedCallback { get; set; }
    }
}