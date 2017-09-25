using System;
using System.Collections.Generic;

namespace Kernel.Notification
{
    [Serializable]
    public class EmailMessage : ICloneable
    {
        [Serializable]
        public class EmailAttachment
        {
            public string AttachmentName { get; set; }
            public string MimeType { get; set; }
            public byte[] Attachment { get; set; }
        }

        public EmailMessage()
        {
            Attachments = new List<EmailAttachment>();
        }

        public string From { get; set; }
        public string[] To { get; set; }
        public string[] Cc { get; set; }
        public string[] Bcc { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool MessageIsHtml { get; set; }
        public List<EmailAttachment> Attachments { get; set; }

        public object Clone()
        {
            var newMessage = (EmailMessage)this.MemberwiseClone();

            newMessage.Attachments = new List<EmailAttachment>();
            
            return newMessage;
        }
    }
}