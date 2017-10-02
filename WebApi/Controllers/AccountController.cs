using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        [Route("SSOLogon")]
        public async Task<IHttpActionResult> SSOLogon()
        {
            var identity = base.RequestContext.Principal.Identity;
            return Ok(((ClaimsIdentity)identity).Claims);
        }
    }
}