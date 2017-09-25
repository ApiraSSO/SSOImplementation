using System;
using System.Net;

namespace Data.Importing.Infrastructure.Resources
{
    public class ResourceConfiguration
    {
        public WebRequest Request { get; private set; }
        public Uri Uri { get; private set; }
        public string Method { get; private set; }
    }
}