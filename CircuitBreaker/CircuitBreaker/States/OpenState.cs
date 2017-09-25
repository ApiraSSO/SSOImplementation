using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircuitBreakerInfrastructure;

namespace CircuitBreaker.States
{
    internal class OpenState : BreakerState
    {
        public OpenState()
        {
        }

        public override State State
        {
            get
            {
                return State.Open;
            }
        }

        public override IExecutionResult Execute(BreakerExecutionContext executionContext)
        {
            throw new NotImplementedException();
        }
    }
}