using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kernel.Federation.Protocols.Response;

namespace Federation.Protocols.Response
{
    internal class ResponseHandler : IReponseHandler
    {
        public Task Handle(Func<IDictionary<string, string>> parser)
        {
            var elements = parser();
            throw new NotImplementedException();
        }
    }
}