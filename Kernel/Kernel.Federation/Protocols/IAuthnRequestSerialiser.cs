using System.Threading.Tasks;
using Kernel.Serialisation;

namespace Kernel.Federation.Protocols
{
    public interface IAuthnRequestSerialiser : ISerializer
    {
        new Task<string> Serialize(object o);
    }
}