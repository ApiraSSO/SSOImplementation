using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Kernel.Serialisation;

namespace Data.Importing.Infrastructure.Resources
{
    public abstract class Resource
    {
        public abstract Task<Stream> GetStream();
    }
}