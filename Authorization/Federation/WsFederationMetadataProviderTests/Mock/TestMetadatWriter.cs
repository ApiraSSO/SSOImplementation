using System;
using System.Xml;
using Kernel.Federation.MetaData;

namespace WsFederationMetadataProviderTests.Mock
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