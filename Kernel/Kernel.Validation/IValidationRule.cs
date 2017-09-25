using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Validation
{
    public interface IValidationRule
    {
        Task Validate(ValidationContext context, Func<ValidationContext, Task> next);
    }
}