using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Importing.Infrastructure
{
    public abstract class ImportStage
    {
        public abstract ImportStages Stage { get; }
    }
}
