using System;
using System.Collections.Generic;
using Kernel.Data.ORM;

namespace ORMMetadataContextProvider
{
    internal class DbCustomConfiguration : IDbCustomConfiguration
    {
        public DbCustomConfiguration()
        {
            this.Seeders = new List<ISeeder>();
        }
        public ICollection<ISeeder> Seeders { get; }

        public Func<IEnumerable<Type>> ModelsFactory { get; set; }
    }
}