
namespace Kernel.Data
{
	public interface IHasID<TID> : IHasID
    {
		TID Id { get; }
	}
}