using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kernel.Federation.Protocols.Response
{
    public interface IReponseHandler<TResult>
    {
        Task<TResult> Handle(Func<IDictionary<string, string>> parser);
    }
}