using System;
using System.Threading.Tasks;
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

        public Task Write(XmlElement xml, MetadataPublishContext target)
        {
              this._action(xml);
            return Task.CompletedTask;
        }
    }
}