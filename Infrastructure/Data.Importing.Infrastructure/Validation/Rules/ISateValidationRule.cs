using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kernel.Validation;

namespace Data.Importing.Infrastructure.Validation.Rules
{
    public interface ISateValidationRule<TState> : IValidationRule where TState : ImportState
    {
    }
}