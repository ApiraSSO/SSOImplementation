using System.Runtime.Serialization;
namespace Kernel.WCF.Faults
{
    [DataContract]
    public abstract class AbstractFault
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [DataMember]
        public string Message { get; set; }
    }
}