
namespace DataModels
{
	public class Server : Asset
	{
		public bool Owned { get; set; }
		public ushort NumberOfSlots { get; set; }
		public ushort? SlotNumber { get; set; }
		public virtual Cabinet Cabinet { get; set; }

		public override MaterialType MaterialType
		{
			get { return DataModels.MaterialType.Network; }
		}
	}
}