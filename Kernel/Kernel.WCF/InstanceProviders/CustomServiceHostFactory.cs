namespace Kernel.WCF
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using Kernel.Initialisation;

    public class CustomServiceHostFactory : ServiceHostFactory
    {
		public CustomServiceHostFactory()
        {
        }

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
			return new CustomServiceHost(ApplicationConfiguration.Instance.DependencyResolver, serviceType, baseAddresses);
        }
    }
}