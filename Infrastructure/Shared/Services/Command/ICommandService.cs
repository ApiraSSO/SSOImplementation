using System.ServiceModel;
using System.Threading.Tasks;
using Kernel.CQRS.Messaging;

namespace Shared.Services.Command
{
    [ServiceContract]
	public interface ICommandService
	{
		/// <summary>
		/// Processes the command.
		/// </summary>
		/// <typeparam name="TMessage">The type of the command.</typeparam>
		/// <param name="command">The command.</param>
		/// <returns></returns>
		Task<object> ProcessCommand<TMessage>(TMessage command) where TMessage : Message;
	}
}
