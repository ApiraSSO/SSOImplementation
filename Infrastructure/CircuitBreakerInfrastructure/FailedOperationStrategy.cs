using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitBreakerInfrastructure
{
    public abstract class FailedOperationStrategy
    {
        public abstract void Apply(FailedOperationContext context, Action<FailedOperationContext> next);
    }
}
