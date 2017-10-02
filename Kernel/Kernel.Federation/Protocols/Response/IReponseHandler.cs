using System.Threading.Tasks;
using Kernel.Federation.Protocols.Bindings.HttpPostBinding;

namespace Kernel.Federation.Protocols.Response
{
    public interface IReponseHandler<TResult>
    {
        Task<TResult> Handle(HttpPostResponseContext context);
    }
}