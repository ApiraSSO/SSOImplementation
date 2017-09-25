using System.Threading.Tasks;
using Kernel.Data;

namespace Kernel.Authentication.Services
{
    public interface ITokenProviderService<TUser, TKey> where TUser : class, IHasID
    {
        Task<string> GenerateUserToken(string purpose, TUser user);
    }
}