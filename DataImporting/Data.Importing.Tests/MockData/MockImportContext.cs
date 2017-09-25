using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Importing.Infrastructure;
using Data.Importing.Infrastructure.Contexts;
using Kernel.Data.DataRepository;

namespace Data.Importing.Tests.MockData
{
    public class MockImportContext : ImportContext
    {
        public MockImportContext(SourceContext source, TargetContext target) : base(source, target)
        {
        }
    }
}
