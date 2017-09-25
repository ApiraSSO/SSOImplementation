using System;
using Nest;

namespace ElasticSearchClient.ErrorHandling
{
    internal class ResponseHandler : IResponseHandler
    {
        public void ValdateAndHandleException(IResponse response, bool throwOnError)
        {
            if (response.IsValid)
                return;

            //ToDo: build a proper error message
            if (throwOnError && response.OriginalException != null)
                throw new Exception(response.DebugInformation, response.OriginalException);
        }
    }
}