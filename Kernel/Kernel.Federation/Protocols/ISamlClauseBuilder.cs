using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Federation.Protocols
{
    public interface ISamlClauseBuilder
    {
        uint Order { get; }
        Task Build(BindingContext context);
    }
}