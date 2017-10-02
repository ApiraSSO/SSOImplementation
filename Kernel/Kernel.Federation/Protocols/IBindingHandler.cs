using System.Threading.Tasks;

namespace Kernel.Federation.Protocols
{
    public interface IBindingHandler
    {
        Task BuildRequest(BindingContext context);
        Task HandleResponse<TResponse>(TResponse response);
    }
}