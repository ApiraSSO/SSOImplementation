using System.Linq;
using System.Runtime.Serialization;
using Kernel.Extensions;

namespace Shared.Services.Responses.Query
{
	[DataContract]
	public abstract class AbstractResponse
	{
		private ResponseStatuses _status = ResponseStatuses.Success;

		public AbstractResponse()
		{
			ResponseMessages = new ResponseMessageCollection();
		}

		/// <summary>
		/// Gets or sets the status of the response.
		/// </summary>
		/// <value>
		/// The status.
		/// </value>
		[DataMember]
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
		[DataMember]
		public ResponseMessageCollection ResponseMessages { get; set; }

		/// <summary>
		/// Gets the aggragated message.
		/// </summary>
		/// <returns></returns>
		public string GetAggragatedMessage()
		{
			if (ResponseMessages == null)
				return string.Empty;

			var messages = ResponseMessages.Select(m => m.Message);

			return EnumerableExtensions.Aggregate(messages);
		}

		/// <summary>
		/// Fails the with message.
		/// </summary>
		/// <param name="message">The message.</param>
		public void FailWithMessage(string message)
		{
			Status = ResponseStatuses.Failure;

			AddError(message);
		}

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
	}
}
