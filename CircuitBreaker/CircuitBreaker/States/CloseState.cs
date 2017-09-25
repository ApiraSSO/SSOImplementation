using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircuitBreakerInfrastructure;

namespace CircuitBreaker.States
{
    internal class CloseState : BreakerState
    {
        public CloseState()
        {
        }

        public override State State
        {
            get
            {
                return State.Close;
            }
        }

        public override IExecutionResult Execute(BreakerExecutionContext executionContext)
        {
            throw new NotImplementedException();
        }
    }
}
