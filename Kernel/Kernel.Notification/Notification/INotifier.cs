using System;

namespace Kernel.Notification
{
    public interface INotifier : IDisposable
    {
		void SendNotification(object notificationMessage);
    }

    public interface INotifier<T, TMessage> : INotifier where T : class, INotificationSender
    {
        T Sender { get; }
		void SendNotification(TMessage notificationMessage);
    }
}
