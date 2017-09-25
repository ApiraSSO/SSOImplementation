using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircuitBreaker.BreakerResponses;
using CircuitBreakerInfrastructure;

namespace CircuitBreaker.ExecutionResults
{
    internal abstract class ExecutionResult : IExecutionResult
    {
        public IBrakerResponse Execute(IBreakerProxy breaker)
        {
            this.ExcuteInternal(breaker);
            return this.BuildResponse();
        }

        protected abstract IBrakerResponse BuildResponse();

        protected abstract void ExcuteInternal(IBreakerProxy breaker);
    }
}