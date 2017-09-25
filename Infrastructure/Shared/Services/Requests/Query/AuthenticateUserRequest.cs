using System.Runtime.Serialization;

namespace Shared.Services.Requests.Query
{
	[DataContract]
	public class AuthenticateUserRequest
	{
		[DataMember]
		public string Username { get; set; }

		[DataMember]
		public string Password { get; set; }
	}
}
