using System.IO;
using System.Text;
using System.Xml;
using Kernel.Federation.MetaData;

namespace WebClientMetadataWriter
{
    internal class HttpMetadataWriter : IFederationMetadataWriter
    {
        public void Write(XmlElement xml, Stream target)
        {
            var writer = new StreamWriter(target);
            using (var w = XmlWriter.Create(writer, new XmlWriterSettings { Encoding = Encoding.UTF8 }))
            {
                xml.WriteTo(w);
            }
        }
    }
}