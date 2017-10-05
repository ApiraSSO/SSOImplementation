using System;
using System.Xml.Serialization;
using Shared.Federtion.Constants;

namespace Shared.Federtion.Models
{
    //[XmlInclude(typeof(ProxyRestriction))]
    //[XmlInclude(typeof(OneTimeUse))]
    [XmlInclude(typeof(AudienceRestriction))]
    [Serializable]
    //[DebuggerStepThrough]
    [XmlType(Namespace = Saml20Constants.Assertion)]
    [XmlRoot(ElementName, Namespace = Saml20Constants.Assertion, IsNullable = false)]
    public abstract class ConditionAbstract
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ElementName = "Condition";
    }
}
