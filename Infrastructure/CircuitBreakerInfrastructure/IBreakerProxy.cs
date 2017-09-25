using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitBreakerInfrastructure
{
    public interface IBreakerProxy
    {
        BreakerState CurrentState { get; }
        void Open();
        void HalfOpen();
        void Close();
        IBrakerResponse Execute(BreakerExecutionContext executionContext);
    }
}