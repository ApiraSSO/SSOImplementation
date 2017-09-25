
using Kernel.Data;

namespace DataModels
{
	public abstract class Asset : BaseModel
	{
		public string Name { get; set; }
		public abstract MaterialType MaterialType { get; }
		public virtual Location Location { get; set; }
	}
}