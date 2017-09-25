using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Importing.Infrastructure;
using Data.Importing.Infrastructure.Configuration;
using Data.Importing.Infrastructure.Factories;

namespace Data.Importing.Factories
{
    internal class ImporterFactory : IImporterFactory
    {
        public IImporter GetImporter(ImportConfiguration configuration)
        {
            throw new NotImplementedException();
        }
    }
}
