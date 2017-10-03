using System.Threading.Tasks;
using Kernel.Serialisation;

namespace Kernel.Federation.Protocols
{
    public interface IRelayStateSerialiser : ISerializer
    {
        new Task<object> Deserialize(string data);
        new Task<string> Serialize(object data);
    }
}