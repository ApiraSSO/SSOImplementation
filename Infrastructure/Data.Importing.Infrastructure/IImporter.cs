using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Importing.Infrastructure.Contexts;

namespace Data.Importing.Infrastructure
{
    public interface IImporter
    {
        void Import(ImportContext context);
    }
}
