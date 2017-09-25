namespace Kernel.CQRS
{
	using System.Threading.Tasks;
    using Kernel.CQRS.Messaging;

    /// <summary>
    /// Exposes method to process serialized command
    /// </summary>
    public interface IMessageProcessor
    {
		Task ProcessMessage(string command);
		Task ProcessProcess(Message command);
	}
}