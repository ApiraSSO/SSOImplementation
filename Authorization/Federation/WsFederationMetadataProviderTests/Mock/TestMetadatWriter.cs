using System;
using System.IO;
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

        public void Write(XmlElement xml, Stream target)
        {
              this._action(xml);
        }
    }
}