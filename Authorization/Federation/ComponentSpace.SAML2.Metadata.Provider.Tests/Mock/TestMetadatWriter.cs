using System;
using System.Xml;
using Kernel.Federation.MetaData;

namespace ComponentSpace.SAML2.Metadata.Provider.Tests.Mock
{
    internal class TestMetadatWriter : IFederationMetadataWriter
    {
        private Action<XmlElement> _action;
        public TestMetadatWriter(Action<XmlElement> action)
        {
            this._action = action;
        }

        public void Write(XmlElement xml)
        {
              this._action(xml);
        }
    }
}