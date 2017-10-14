using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared.Federtion.Response
{
    public class ResponseStatus
    {
        public ResponseStatus()
        {
            this.StatusCodes = new List<string>();
            this.StatusMessage = String.Empty;
            this.MessageDetails = String.Empty;
        }
        public ICollection<string> StatusCodes { get; }
        public string StatusMessage { get; set; }
        public string MessageDetails  { get; set; }
        public bool IsSuccess
        {
            get
            {
                return this.StatusCodes != null && this.StatusCodes.Any(x => x == Shared.Federtion.Constants.StatusCodes.Success);
            }
        }
        public string StatusCodeMain
        {
            get
            {
                if (this.StatusCodes == null)
                    throw new ArgumentNullException("statusCodes");
                if (this.StatusCodes.Count == 0)
                    throw new InvalidOperationException("No status code available. Parse the response status.");
                return this.StatusCodes.ElementAt(0);
            }
        }
        public string AdittionalStatusCode
        {
            get
            {
                if (this.StatusCodes == null)
                    throw new ArgumentNullException("statusCodes");
                if (this.StatusCodes.Count == 0)
                    throw new InvalidOperationException("No status code available. Parse the response status.");
                return this.StatusCodes.Count == 2 ? this.StatusCodes.ElementAt(1) : null;
            }
        }

        public string AggregatedMessage
        {
            get
            {
                var sb = new StringBuilder();
                sb.AppendFormat("StatusCode: {0}\r\n", this.StatusCodeMain);

                if (!String.IsNullOrWhiteSpace(this.AdittionalStatusCode))
                    sb.AppendFormat("Additional status code: {0}\r\n", this.AdittionalStatusCode);

                if (!String.IsNullOrWhiteSpace(this.StatusMessage))
                    sb.AppendFormat("Message status: {0}\r\n", this.StatusMessage);

                if (!String.IsNullOrWhiteSpace(this.MessageDetails))
                    sb.AppendFormat("Message details: {0}", this.MessageDetails);
                
                return sb.ToString();
            }
        }
    }
}