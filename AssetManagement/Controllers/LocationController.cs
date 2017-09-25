using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using DataModels;
using Shared.Services.Query.Location;

namespace AssetManagement.Controllers
{
    [Authorize]
	[RoutePrefix("odata/Locations")]
	public class LocationController : BaseController<ILocationQueryService>
    {
		public LocationController(ILocationQueryService service) : base(service) { }

        [HttpGet]
		public async Task<IHttpActionResult> Get()
        {
			//search criteria to be passed. TBD
			//validation goes here
			//ideally a projection should be returned
			var locations = await base.Service.GetAllLoction();
			return base.Ok(locations);
        }

       [HttpGet]
		public async Task<IHttpActionResult> Get(Guid id)
        {
			throw new NotImplementedException();
        }
    }
}