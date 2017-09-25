using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Data.Importing.Infrastructure.Resources.FtpResource
{
    public class FtpResource : Resource
    {
        public override Task<Stream> GetStream()
        {
            throw new NotImplementedException();
        }
    }
}