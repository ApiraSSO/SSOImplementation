using System.Collections.Generic;
using System.Linq;

namespace Kernel.Messaging.Response
{
    public sealed class ResponseMessageCollection : List<ResponseMessage>
    {
        /// <summary>
        /// Adds the specified code.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageType">Type of the message.</param>
        public void Add(string message, ResponseMessageTypes messageType)
        {
            Add(new ResponseMessage(message, messageType));
        }

        public void AddError(string message)
        {
            Add(ResponseMessage.NewError(message));
        }

        public void AddWarning(string message)
        {
            Add(ResponseMessage.NewWarning(message));
        }

        public void AddInformation(string message)
        {
            Add(ResponseMessage.NewInformation(message));
        }

        /// <summary>
        /// Gets a value indicating whether this instance has errors.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has errors; otherwise, <c>false</c>.
        /// </value>
        public bool HasErrors
        {
            get
            {
                return HasMessagesOfType(ResponseMessageTypes.Error);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has warnings.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has warnings; otherwise, <c>false</c>.
        /// </value>
        public bool HasWarnings
        {
            get
            {
                return HasMessagesOfType(ResponseMessageTypes.Warning);
            }
        }


        /// <summary>
        /// Determines whether [has messages of type] [the specified message type].
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <returns>
        ///   <c>true</c> if [has messages of type] [the specified message type]; otherwise, <c>false</c>.
        /// </returns>
        private bool HasMessagesOfType(ResponseMessageTypes messageType)
        {
            return this.Where(m => m.MessageType == messageType).Count() > 0;
        }
    }
}