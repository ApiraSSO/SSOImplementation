using System.Threading.Tasks;
using Kernel.CQRS.Messaging;

namespace CQRS.Infrastructure
{
    public interface IMessageService<TMessage> where TMessage : Message
    {
        /// <summary>
        /// Processes the command.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        //Task ProcessMessage(string message);

        /// <summary>
        /// Processes the command.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        Task ProcessMessage(TMessage message);
    }
}