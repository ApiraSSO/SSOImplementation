using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services.Responses.Query
{
	[DataContract]
	public class AuthenticateUserResponse : BaseAuthenticateUserResponse
	{
		[DataMember]
		public DateTimeOffset? LastLogin { get; set; }

		[DataMember]
		public bool MustChangePassword { get; set; }
	}
}
