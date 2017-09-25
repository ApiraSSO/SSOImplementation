using System;
using System.Xml.Serialization;

namespace Federation.Protocols.Request
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
