using System;
using System.Net;
using Kernel.Authentication;

namespace Data.Importing.Infrastructure.Resources
{
    public class WebResourceConfiguration : ResourceConfiguration
    {
        public ICredentialsProvider CredentialsProvider { get; private set; }
    }
}