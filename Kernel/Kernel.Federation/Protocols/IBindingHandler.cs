using System.Threading.Tasks;

namespace Kernel.Federation.Protocols
{
    public interface IBindingHandler
    {
        Task HandleRequest(SamlRequestContext context);
        Task HandleResponse(SamlResponseContext context);
    }
}