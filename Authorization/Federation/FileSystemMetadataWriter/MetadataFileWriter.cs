using System;
using System.IO;
using System.Xml;
using Kernel.Federation.FederationPartner;
using Kernel.Federation.MetaData;

namespace FileSystemMetadataWriter
{
    internal class MetadataFileWriter : IFederationMetadataWriter
    {
        IFederationPartyContextBuilder federationPartyContextBuilder;
        public MetadataFileWriter(IFederationPartyContextBuilder federationPartyContextBuilder)
        {

        }
        public void Write(XmlElement xml, Stream target)
        {
            throw new NotImplementedException();

            //if (File.Exists(path))
            //    File.Delete(path);

            //using (var writer = XmlWriter.Create(path))
            //{
            //    xml.WriteTo(writer);
            //    writer.Flush();
            //}
        }
    }
}