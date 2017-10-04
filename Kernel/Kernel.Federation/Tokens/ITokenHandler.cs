using System.Threading.Tasks;

namespace Kernel.Federation.Tokens
{
    public interface ITokenHandler
    {
        Task<TokenHandlingResponse> HandleToken(HandleTokenContext context);
    }
}