using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Federation.Protocols.Response
{
    public interface IReponseHandler
    {
        Task Handle(Func<IDictionary<string, string>> parser);
    }
}