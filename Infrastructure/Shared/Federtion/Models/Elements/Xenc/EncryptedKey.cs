using System;
using System.Xml.Serialization;
using Shared.Federtion.Constants;

namespace Shared.Federtion.Models
{
    [Serializable]
    [XmlType(Namespace = Saml20Constants.Xenc)]
    [XmlRoot(ElementName, Namespace = Saml20Constants.Xenc, IsNullable = false)]
    public class EncryptedKey : Encrypted
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ElementName = "EncryptedKey";

        #region Attributes

        /// <summary>
        /// Gets or sets the recipient.
        /// </summary>
        /// <value>The recipient.</value>
        [XmlAttribute("Recipient")]
        public string Recipient { get; set; }

        #endregion

        #region Elements

        /// <summary>
        /// Gets or sets the name of the carried key.
        /// </summary>
        /// <value>The name of the carried key.</value>
        [XmlElement("CarriedKeyName")]
        public string CarriedKeyName { get; set; }

        /// <summary>
        /// Gets or sets the reference list.
        /// </summary>
        /// <value>The reference list.</value>
        [XmlElement("ReferenceList")]
        public ReferenceList ReferenceList { get; set; }

        #endregion
    }
}
