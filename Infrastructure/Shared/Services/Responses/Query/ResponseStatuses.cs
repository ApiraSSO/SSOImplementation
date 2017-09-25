using System.Runtime.Serialization;

namespace Shared.Services.Responses.Query
{
	[DataContract]
	public enum ResponseStatuses
	{
		[EnumMember]
		Success,
		[EnumMember]
		Failure,
		[EnumMember]
		Exception
	}
}