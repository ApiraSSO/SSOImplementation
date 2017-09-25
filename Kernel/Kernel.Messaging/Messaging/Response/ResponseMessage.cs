namespace Kernel.Messaging.Response
{
    public sealed class ResponseMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseMessage"/> class.
        /// </summary>
        public ResponseMessage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseMessage"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="type">The type.</param>
        public ResponseMessage(string message, ResponseMessageTypes type)
        {
            MessageType = type;
            Message = message;
        }

        /// <summary>
        /// Gets or sets the message text.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        /// <value>
        /// The type of the message.
        /// </value>
        public ResponseMessageTypes MessageType { get; set; }

        /// <summary>
        /// Factory for a new error message.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static ResponseMessage NewError(string message)
        {
            return new ResponseMessage(message, ResponseMessageTypes.Error);
        }

        /// <summary>
        /// Factory for a new warning message.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static ResponseMessage NewWarning(string message)
        {
            return new ResponseMessage(message, ResponseMessageTypes.Warning);
        }

        /// <summary>
        /// Factory for a new information message.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static ResponseMessage NewInformation(string message)
        {
            return new ResponseMessage(message, ResponseMessageTypes.Infomation);
        }
    }
}