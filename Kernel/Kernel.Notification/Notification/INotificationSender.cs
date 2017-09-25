
namespace Kernel.Notification
{
    public interface INotificationSender
    {
        IDestinationTargetProvider DestinationTarget { get; set; }
        void Init();
    }

    public interface INotificationSender<T> : INotificationSender
    {
        void Send(T notification);
    }
}
