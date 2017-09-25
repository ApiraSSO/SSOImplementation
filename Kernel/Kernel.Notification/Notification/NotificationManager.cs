namespace Kernel.Notification
{
    using System.Linq;
    using Kernel.DependancyResolver;
    using Kernel.Logging;

    public abstract class NotificationManager
    {
        IDependencyResolver _resolver;
        public NotificationManager(IDependencyResolver resolver)
        {
            this._resolver = resolver;
        }


        protected virtual void SendNotification(object notificationMessage)
        {
            var notifiers = _resolver.ResolveAll<INotifier>();

            if (notifiers == null || notifiers.Count() == 0)
            {
                LoggerManager.WriteWarningToEventLog("No objects implementing INotifier have been found in DI container. Make sure they are registered in the container or implement some of the auto - requster interfaces");

                return;
            }

            foreach (var notifier in notifiers)
            {
                using (notifier)
                {
                    notifier.SendNotification(notificationMessage);
                }
            }
        }
    }
}