using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Validation
{
    public interface IValidationRuleFactory
    {
        IEnumerable<IValidationRule> GetValidationRules(Type type);
        IEnumerable<IValidationRule> GetValidationRules<T>(Func<T, IEnumerable<object>> resolver);
        IEnumerable<IValidationRule> GetValidationRules<T>();
        IEnumerable<IValidationRule> GetValidationRules<T>(Func<T, bool> filter);
    }
}
