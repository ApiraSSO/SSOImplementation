using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitBreakerInfrastructure
{
    public interface IStateManager
    {
        BreakerState State { get; }
        
        IExecutionResult Execute(BreakerExecutionContext executionContext);
    }
}