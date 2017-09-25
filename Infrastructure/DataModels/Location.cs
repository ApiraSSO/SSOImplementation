using System.Collections.Generic;
using Kernel.Data;

namespace DataModels
{
	public class Location : BaseModel
	{
		public Location()
		{
			this.ChildrenLocations = new List<Location>();
		}

		public string Name { get; set; }
		public LocationType LocationType { get; set; }
		public virtual Location ParentLocation { get; set; }
		public virtual ICollection<Location> ChildrenLocations { get; protected set; }
	}
}
