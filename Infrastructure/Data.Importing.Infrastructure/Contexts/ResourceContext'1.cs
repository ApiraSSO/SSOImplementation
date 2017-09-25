using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Data.Importing.Infrastructure.Resources;
using Kernel.Authentication;
using Kernel.Data.DataRepository;
using Kernel.Data.ORM;

namespace Data.Importing.Infrastructure.Contexts
{
    public class WebResourceContext<TResource> : ResourceContext where TResource : Resource
    {
        TResource _resource;
        public WebResourceContext(ICredentialsProvider credentialsProvider, TResource resource) : base(credentialsProvider)
        {
            this._resource = resource;
        }

        protected async override Task<Stream> GetStreamInternal()
        {
             return await this._resource.GetStream();
        }
    }
}