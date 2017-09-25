using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Federation.MetaData.Configuration.EndPoint
{
    public class EndPointConfiguration
    {
        public Uri Binding { get; set; }
        public Uri Location { get; set; }
        public Uri ResponseLocation { get; set; }
    }
}
