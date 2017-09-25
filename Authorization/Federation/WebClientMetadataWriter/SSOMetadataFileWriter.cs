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

            var sb = new StringBuilder();
            using (var w = XmlWriter.Create(sb))
            {
                xml.WriteTo(w);
            }

            HttpContext.Current.Response.ContentType = "text/xml";
            HttpContext.Current.Response.Write(sb.ToString());
        }
    }
}