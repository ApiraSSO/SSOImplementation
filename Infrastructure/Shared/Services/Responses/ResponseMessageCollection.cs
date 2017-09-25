using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Shared.Services.Responses
{
	[CollectionDataContract]
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

		/// <summary>
		/// Flattens the messages.
		/// </summary>
		/// <returns></returns>
		private string FlattenMessages()
		{
			var sb = new StringBuilder();
			foreach (var message in this)
			{
				sb.AppendLine(message.ToString());
			}
			return sb.ToString();
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return FlattenMessages();
		}
	}
}
