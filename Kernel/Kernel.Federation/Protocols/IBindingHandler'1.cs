using System.Threading.Tasks;

namespace Kernel.Federation.Protocols
{
    public interface IBindingHandler<TContext> : IBindingHandler where TContext : BindingContext
    {
        Task BuildRequest(TContext context);
    }
}