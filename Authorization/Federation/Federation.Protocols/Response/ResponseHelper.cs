using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Shared.Federtion.Constants;

namespace Federation.Protocols.Response
{
    internal class ResponseHelper
    {
        internal static void EnsureSuccess(string response)
        {
            using (var reader = new StringReader(response))
            {
                using (var xmlReader = XmlReader.Create(reader))
                {
                    ResponseHelper.ValidateResponseSuccess(xmlReader);
                }
            }
        }
        
        internal static void ValidateResponseSuccess(XmlReader reader)
        {
            var statusMessage = String.Empty;
            var messageDetails = String.Empty;
            var statusCodes = new List<string>();

            while (!reader.IsStartElement("Status", Saml20Constants.Protocol))
            {
                if (!reader.Read())
                    throw new InvalidOperationException("Can't find status code element.");
            }
            
            while (reader.Read() && !(reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "Status"))
            {
                if (reader.IsStartElement("StatusCode", Saml20Constants.Protocol))
                {
                    var statusCode = reader.GetAttribute("Value");
                    if (!String.IsNullOrWhiteSpace(statusCode))
                        statusCodes.Add(statusCode);
                    continue;
                }
                
                if (reader.IsStartElement("StatusMessage", Saml20Constants.Protocol))
                {
                    reader.Read();
                    statusMessage = reader.Value;
                    continue;
                }

                if (reader.IsStartElement("StatusDetail", Saml20Constants.Protocol))
                {
                    reader.Read();
                    statusMessage = reader.Value;
                    continue;
                }
            }

            if (statusCodes.Count == 1 && statusCodes.SingleOrDefault(x => x == StatusCodes.Success) != null)
                return;

            var sb = new StringBuilder();
            statusCodes.Aggregate(sb, (b, next) =>
            {
                b.AppendFormat("Status code: {0}\r\n", next);
                return b;
            });

            var msg = String.Format("{0}\r\nAdditional information:{1} {2}", sb.ToString(), statusMessage, messageDetails);
            throw new Exception(msg);
            
        }
    }
}