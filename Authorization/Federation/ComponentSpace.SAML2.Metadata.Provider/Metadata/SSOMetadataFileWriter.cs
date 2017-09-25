using System;
using System.Xml;
using Kernel.Federation.MetaData;

namespace ComponentSpace.SAML2.Metadata.Provider.Metadata
{
    public class SSOMetadataFileWriter : IFederationMetadataWriter
    {
        public void Write(XmlElement xml)
        {
            throw new NotImplementedException();
            //using (var w = XmlWriter.Create(configuration.MetadatFilePathDestination))
            //{
            //    xml.WriteTo(w);
            //}
        }
    }
}