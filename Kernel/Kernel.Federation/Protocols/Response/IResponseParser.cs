using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Federation.Protocols.Response
{
    public interface IResponseParser<TResponse>
    {
        IDictionary<string, string> ParseResponse(TResponse response);
    }
}