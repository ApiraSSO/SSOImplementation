using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircuitBreaker.BreakerResponses;
using CircuitBreakerInfrastructure;

namespace CircuitBreaker.ExecutionResults
{
    internal class OpenExecutionResult : ExecutionResult
    {
        protected override IBrakerResponse BuildResponse()
        {
            return new BrakerResponse();
        }

        protected override void ExcuteInternal(IBreakerProxy breaker)
        {
            breaker.Open();
        }
    }
}