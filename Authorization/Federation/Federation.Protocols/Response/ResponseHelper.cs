using System;
using System.IO;
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
            var statusCode = String.Empty;
            var statusCodeSub = String.Empty;
            while (!reader.IsStartElement("Status", Saml20Constants.Protocol))
            {
                if (!reader.Read())
                    throw new InvalidOperationException("Can't find status code element.");
            }
            reader.Read();
            if (!reader.IsStartElement("StatusCode", Saml20Constants.Protocol))
                throw new InvalidOperationException("Excpected element StatusCode");

            statusCode = reader.GetAttribute("Value");
            if (!String.IsNullOrWhiteSpace(statusCode) && String.Equals(statusCode, StatusCodes.Success))
                return;

            reader.Read();
            if (reader.IsStartElement("StatusCode", Saml20Constants.Protocol))
            {
                statusCodeSub = reader.GetAttribute("Value");
                reader.Read();
            }
            reader.Read();
            if (reader.IsStartElement("StatusMessage", Saml20Constants.Protocol))
            {
                reader.Read();
                statusMessage = reader.Value;
                reader.Read();
            }
            reader.Read();
            if (reader.IsStartElement("StatusDetail", Saml20Constants.Protocol))
            {
                reader.Read();
                statusMessage = reader.Value;
            }
            
            if (String.IsNullOrWhiteSpace(statusCode) || !String.Equals(statusCode, StatusCodes.Success))
            {
                var msg = String.Format("{0}.\r\n Additional information:{1}, {2}, {3}", statusCode, statusCodeSub, statusMessage, messageDetails);
                throw new Exception(msg);
            }
        }
    }
}