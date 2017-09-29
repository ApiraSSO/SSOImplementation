using System;
using System.IO;
using System.Text;
using System.Xml;
using Kernel.Federation.MetaData;

namespace Shared.Federtion
{
    public abstract class MetadataWriter : IFederationMetadataWriter
    {
        public void Write(XmlElement xml, MetadataPublishContext target)
        {
            if (xml == null)
                throw new ArgumentNullException("xmlElement");

            if (target == null)
                throw new ArgumentNullException("target");
            
            if (!this.CanWrite(target))
                return;

            var writer = new StreamWriter(target.TargetStream);
            using (var w = XmlWriter.Create(writer, new XmlWriterSettings { Encoding = Encoding.UTF8 }))
            {
                xml.WriteTo(w);
            }
        }

        protected abstract bool CanWrite(MetadataPublishContext target);
    }
}