using System.Net;

namespace Kernel.Notification
{
    public interface IDestinationTargetProvider
    {
        NetworkCredential NetworkCredential { get; }
        string Path { get; }
        int Port { get; }
        bool SSL { get; }
    }
}
