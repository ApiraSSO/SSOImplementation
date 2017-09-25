using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CircuitBreaker.States;
using CircuitBreakerInfrastructure;

namespace CircuitBreaker.BreakerProxy
{
    internal abstract class BreakerProxy : IBreakerProxy
    {
        private IStateProvider _stateProvider;
        private IStateManager _stateManager;
        protected BreakerProxy(IStateProvider stateProvider)
        {
            this._stateProvider = stateProvider;
        }

        public BreakerState CurrentState => this._stateManager.State;

        public void Close()
        {
            var closedState = this._stateProvider.GetState(State.Close);
            var haldOpenState = this._stateProvider.GetState(State.HalfOpen);
            Interlocked.CompareExchange(ref this._stateManager, closedState, haldOpenState);
        }

        public IBrakerResponse Execute(BreakerExecutionContext executionContext)
        {
            var result = this._stateManager.Execute(executionContext);
            return result.Execute(this);
        }

        public void HalfOpen()
        {
            var openState = this._stateProvider.GetState(State.Open);
            var halfOpenState = this._stateProvider.GetState(State.HalfOpen);
            Interlocked.CompareExchange(ref this._stateManager, halfOpenState, openState);
        }

        public void Open()
        {
            var closedState = this._stateProvider.GetState(State.Close);
            var openState = this._stateProvider.GetState(State.Open);
            Interlocked.CompareExchange(ref this._stateManager, openState, closedState);
        }
    }
}
