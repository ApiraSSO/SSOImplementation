using System;
using System.Collections.Generic;

namespace Kernel.Data.ORM
{
    public interface IDbCustomConfiguration
    {
        ICollection<ISeeder> Seeders { get; }
        Func<IEnumerable<Type>> ModelsFactory { get; }
    }
}
