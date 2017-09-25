
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Shared.Services.Query.Assets
{
	public interface IAssetQueryService
	{
		//methods go here
		//example return a projection(doesn't esist yet), not the whole doman object
		//pass search criteria to the methods. not implemented yet
		Task<IEnumerable<DataModels.Asset>> GetAllAsset();
	}
}
