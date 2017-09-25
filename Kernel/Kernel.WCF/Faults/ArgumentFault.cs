using System.Runtime.Serialization;

namespace Kernel.WCF.Faults
{
    [DataContract(Namespace = "http://unitysys.net/common/faults")]
    public class ArgumentFault : AbstractFault
    {
        /// <summary>
        /// Gets or sets the name of the argument.
        /// </summary>
        /// <value>
        /// The name of the argument.
        /// </value>
        [DataMember]
        public string ArgumentName { get; set; }
    }
}