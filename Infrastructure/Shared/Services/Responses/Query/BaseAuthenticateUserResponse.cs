using System;
using System.Runtime.Serialization;
using Kernel.Authentication;

namespace Shared.Services.Responses.Query
{
    [DataContract]
	public class BaseAuthenticateUserResponse : AbstractResponse
	{
		[DataMember]
		public AuthenticationResults SignInResult { get; set; }

		[DataMember]
		public int? UserId { get; set; }

		[DataMember]
		public Guid GlobalUserId { get; set; }

		[DataMember]
		public Guid SessionIdentifier { get; set; }

		[DataMember]
		public string UserName { get; set; }
	}
}
