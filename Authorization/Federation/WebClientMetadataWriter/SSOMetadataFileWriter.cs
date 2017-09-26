using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using Kernel.Federation.MetaData;

namespace WebClientMetadataWriter
{
    internal class SSOMetadataFileWriter : IFederationMetadataWriter
    {
        public void Write(XmlElement xml)
        {
            if (HttpContext.Current == null || HttpContext.Current.Response == null)
                return;
            HttpContext.Current.Response.ContentType = "text/xml";
            
            var writer = new StreamWriter(HttpContext.Current.Response.OutputStream);
            using (var w = XmlWriter.Create(writer, new XmlWriterSettings { Encoding = Encoding.UTF8 }))
            {
                xml.WriteTo(w);
            }
        }
    }
}