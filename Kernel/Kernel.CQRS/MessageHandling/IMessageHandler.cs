namespace Kernel.CQRS.MessageHandling
{
    using System.Threading.Tasks;
    using Kernel.CQRS.Messaging;
    using Kernel.Initialisation;

    /// <summary>
    /// Provides methods to handle commands
    /// </summary>
    /// <typeparam name="TMessage">The type of the command.</typeparam>
    public interface IMessageHandler<TMessage> : IAutoRegisterAsTransient where TMessage : Message
	{
		/// <summary>
		/// Handles the specified command.
		/// </summary>
		/// <param name="command">The command.</param>
		Task Handle(TMessage command);
	}
}