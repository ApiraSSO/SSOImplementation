using System.ServiceModel;
using System.Threading.Tasks;
using Shared.Services.Requests.Query;
using Shared.Services.Responses.Query;

namespace Shared.Services.Query.Authentication
{
	[ServiceContract]
	public interface IAuthenticationQueryService
	{
		[OperationContract]
		Task<AuthenticateUserResponse> AuthenticateUser(AuthenticateUserRequest authenticateUserRequest);
	}
}
