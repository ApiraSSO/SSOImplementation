using Kernel.Initialisation;
using Nest;

namespace ElasticSearchClient.ErrorHandling
{
    public interface IResponseHandler : IAutoRegisterAsTransient
    {
        void ValdateAndHandleException(IResponse response, bool throwOnError);
    }
}