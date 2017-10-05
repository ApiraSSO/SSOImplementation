using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Shared.Federtion.Constants;

namespace Shared.Federtion.Models
{
    [Serializable]
    [XmlType(Namespace = Saml20Constants.Assertion)]
    [XmlRoot(ElementName, Namespace = Saml20Constants.Assertion, IsNullable = false)]
    public class AudienceRestriction : ConditionAbstract
    {
        public AudienceRestriction()
        {
            this.Audience = new List<string>();
        }
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public new const string ElementName = "AudienceRestriction";

        #region Elements

        /// <summary>
        /// Gets or sets the audience.
        /// A URI reference that identifies an intended audience. The URI reference MAY identify a document
        /// that describes the terms and conditions of audience membership. It MAY also contain the unique
        /// identifier URI from a SAML name identifier that describes a system entity
        /// </summary>
        /// <value>The audience.</value>
        [XmlElement("Audience", DataType = "anyURI", Order = 1)]
        public List<string> Audience { get; }

        #endregion
    }
}