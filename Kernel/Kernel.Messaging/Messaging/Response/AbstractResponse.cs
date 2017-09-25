using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Messaging.Response
{
    [Serializable]
    public abstract class AbstractResponse
    {
        private ResponseStatuses _status = ResponseStatuses.Success;
        private ResponseDataStatuses _dataStatus = ResponseDataStatuses.Fresh;

        public AbstractResponse()
        {
            this.ResponseMessages = new ResponseMessageCollection();
        }


        /// <summary>
        /// Gets or sets the data status denoting whether or not the data in the response is stale or fresh.
        /// </summary>
        /// <value>
        /// The data status.
        /// /value>
        public ResponseDataStatuses DataStatus
        {
            get
            {
                return _dataStatus;
            }
            set
            {
                _dataStatus = value;
            }
        }

        /// <summary>
        /// Gets or sets the status of the response.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public ResponseStatuses Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }

        /// <summary>
        /// Gets or sets the response messages.
        /// </summary>
        /// <value>
        /// The response messages.
        /// </value>
        public ResponseMessageCollection ResponseMessages { get; private set; }


        /// <summary>
        /// Adds an error message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void AddError(string message)
        {
            ResponseMessages.AddError(message);
        }

        /// <summary>
        /// Adds a warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void AddWarning(string message)
        {
            ResponseMessages.AddWarning(message);
        }

        /// <summary>
        /// Adds an information message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void AddInformation(string message)
        {
            ResponseMessages.AddInformation(message);
        }

        /// <summary>
        /// Gets a value indicating whether this instance has messages.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has messages; otherwise, <c>false</c>.
        /// </value>
        public bool HasMessages
        {
            get
            {
                return ResponseMessages != null && ResponseMessages.Count > 0;
            }
        }

        /// <summary>
        /// Gets or sets the metrics.
        /// </summary>
        /// <value>
        /// The metrics.
        /// </value>
        public ResponseMetrics Metrics { get; set; }

        /// <summary>
        /// Gets or sets the versions.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public Dictionary<string, string> Versions { get; set; }
    }
}
