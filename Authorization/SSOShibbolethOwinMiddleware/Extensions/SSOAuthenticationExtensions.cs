using System;
using Kernel.Federation.MetaData;
using Kernel.Initialisation;
using Owin;

namespace SSOOwinMiddleware.Extensions
{
    public static class SSOAuthenticationExtensions
    {
        public static IAppBuilder UseShibbolethAuthentication(this IAppBuilder app, SSOAuthenticationOptions options)
        {
            if (app == null)
                throw new ArgumentNullException("app");
            if (options == null)
                throw new ArgumentNullException("options");
            var resolver = ApplicationConfiguration.Instance.DependencyResolver;
            app.Use((object)typeof(SSOOwinMiddleware), (object)app, (object)options, resolver);

            app.Map(options.SPMetadataPath, a =>
            {
                a.Run(c =>
                {
                    var metadataGenerator = SSOAuthenticationExtensions.ResolveMetadataGenerator<ISPMetadataGenerator>();
                    return metadataGenerator.CreateMetadata(MetadataType.SP);

                });
            });
            return app;
        }

        public static IAppBuilder UseShibbolethAuthentication(this IAppBuilder app, string wtrealm)
        {
            return app.UseShibbolethAuthentication(new SSOAuthenticationOptions()
            {
                Wtrealm = wtrealm
            });
        }
        private static TMetadatGenerator ResolveMetadataGenerator<TMetadatGenerator>() where TMetadatGenerator : IMetadataGenerator
        {
            var resolver = ApplicationConfiguration.Instance.DependencyResolver;
            var metadataGenerator = resolver.Resolve<TMetadatGenerator>();
            return metadataGenerator;
        }
    }
}