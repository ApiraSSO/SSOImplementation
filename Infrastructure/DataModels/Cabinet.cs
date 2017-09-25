using System.Collections.Generic;

namespace DataModels
{
	public class Cabinet : Asset
	{
		public ushort FreeContiguousSlots { get; set; }
		public ushort NumberOfSlotsAvailable { get; set; }
		public virtual ICollection<Asset> MountedAssets { get; protected set; }

		public override MaterialType MaterialType
		{
			get { return DataModels.MaterialType.Cabinet; }
		}
	}
}