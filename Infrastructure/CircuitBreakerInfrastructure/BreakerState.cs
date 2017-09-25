using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircuitBreakerInfrastructure;

namespace CircuitBreakerInfrastructure
{
    public abstract class BreakerState
    {
        protected IStateManager StateManager;
        public abstract State State { get; }
        protected BreakerState()
        {
        }

        public abstract IExecutionResult Execute(BreakerExecutionContext executionContext);
    }
}
