using System.Diagnostics;
using System.IO;
using Elasticsearch.Net;

namespace ElasticSearchClient.Connection
{
    internal class RequestInterceptor
    {
        static internal void InterceptRequestDataCreated(RequestData requestData)
        {
#if(DEBUG)
            Debug.WriteLine(requestData.Path);
            if (requestData.PostData == null)
                return;
            
            var streamFactory = requestData.MemoryStreamFactory;
            var ms = streamFactory.Create();
            requestData.PostData.Write(ms, requestData.ConnectionSettings);
            ms.Position = 0;
            var streamTeader = new StreamReader(ms);
            var request = streamTeader.ReadToEnd();
            Debug.WriteLine(request);
#endif
        }

        static internal void InterceptRequestComplete(IApiCallDetails apiCallDetails)
        {
        }
    }
}