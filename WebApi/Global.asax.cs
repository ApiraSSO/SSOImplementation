using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Kernel.Initialisation;
using Kernel.Logging;
using ServerInitialisation;
using UnityResolver;

namespace WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            using (new InformationLogEventWriter())
            {
                ApplicationConfiguration.RegisterDependencyResolver(() => new UnityDependencyResolver());
                ApplicationConfiguration.RegisterServerInitialiserFactory(() => new ServerInitialiser());

                //this.RegisterWebConfiguration();
                this.InitializeServer();
            }
        }

        private void InitializeServer()
        {
            using (new InformationLogEventWriter())
            {
                var container = ApplicationConfiguration.Instance.DependencyResolver;
                var initialiser = ApplicationConfiguration.Instance.ServerInitialiserFactory();
                var task = initialiser.Initialise(container);
            }
        }
    }
}