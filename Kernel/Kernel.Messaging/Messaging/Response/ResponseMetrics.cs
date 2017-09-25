using System;

namespace Kernel.Messaging.Response
{
    public class ResponseMetrics
    {
        /// <summary>
        /// Gets or sets the request recieved.
        /// </summary>
        /// <value>
        /// The request recieved.
        /// </value>
        public DateTimeOffset? RequestRecieved { get; set; }

        /// <summary>
        /// Gets or sets the request completed.
        /// </summary>
        /// <value>
        /// The request completed.
        /// </value>
        public DateTimeOffset? RequestCompleted { get; set; }
    }
}