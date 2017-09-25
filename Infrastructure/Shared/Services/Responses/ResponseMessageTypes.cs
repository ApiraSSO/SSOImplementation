using System.Runtime.Serialization;

namespace Shared.Services.Responses
{
	[DataContract]
	public enum ResponseMessageTypes
	{
		[EnumMember]
		Infomation,
		[EnumMember]
		Error,
		[EnumMember]
		Warning
	}
}
