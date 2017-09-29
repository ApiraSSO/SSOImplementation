using System;
using System.IO;
using System.Xml;
using Kernel.Federation.MetaData;

namespace WsFederationMetadataProvider.Metadata
{
    public class SSOMetadataFileWriter : IFederationMetadataWriter
    {
        public void Write(XmlElement xml, MetadataPublishContext target)
        {
            throw new NotImplementedException();
            //using (var w = XmlWriter.Create(configuration.MetadatFilePathDestination))
            //{
            //    xml.WriteTo(w);
            //}
        }
    }
}