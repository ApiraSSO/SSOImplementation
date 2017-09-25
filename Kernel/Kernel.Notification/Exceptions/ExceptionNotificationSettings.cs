using System;
using System.ComponentModel.DataAnnotations;

namespace Kernel.Notification.Exceptions
{
    public class ExceptionNotificationSettings
    {
        [Required]
        public Type NotificationEntryType { get; set; }
        
        [Required]
        public TimeSpan TimeSpan { get; set; }

        public string[] To { get; set; }
    }
}
