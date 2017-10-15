using System;
using System.IO;
using System.Xml;
using Kernel.Logging;
using Shared.Federtion.Constants;
using Shared.Federtion.Response;

namespace Federation.Protocols.Response
{
    internal class ResponseHelper
    {
        internal static ResponseStatus ParseResponseStatus(string response, ILogProvider logProvider)
        {
            using (var reader = new StringReader(response))
            {
                using (var xmlReader = XmlReader.Create(reader))
                {
                    var responseStatus = ResponseHelper.ReadResponseStatus(xmlReader);
                    logProvider.LogMessage(responseStatus.AggregatedMessage);
                    return responseStatus;
                }
            }
        }
        
        internal static ResponseStatus ReadResponseStatus(XmlReader reader)
        {
            var responseStatus = new ResponseStatus();
            
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
                        responseStatus.StatusCodes.Add(statusCode);
                    continue;
                }
                
                if (reader.IsStartElement("StatusMessage", Saml20Constants.Protocol))
                {
                    reader.Read();
                    responseStatus.StatusMessage = reader.Value;
                    continue;
                }

                if (reader.IsStartElement("StatusDetail", Saml20Constants.Protocol))
                {
                    reader.Read();
                    responseStatus.MessageDetails = reader.Value;
                    continue;
                }
            }

            return responseStatus;
        }

        internal static void EnsureSuccessAndThrow(ResponseStatus status)
        {
            if (status.IsSuccess)
                return;
            var msg = status.AggregatedMessage;
            throw new Exception(msg);
        }
    }
}