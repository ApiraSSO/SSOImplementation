using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircuitBreaker.States;
using CircuitBreakerInfrastructure;

namespace CircuitBreaker.StateManagers
{
    internal class CloseStateManager : StateManager<CloseState>
    {
        public CloseStateManager(CloseState state, ITimeManager timeManager, IBreakerProxy breaker) : base(state, timeManager, breaker)
        {
        }
    }
}