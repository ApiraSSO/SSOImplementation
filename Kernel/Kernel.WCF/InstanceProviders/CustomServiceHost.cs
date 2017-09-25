namespace Kernel.WCF
{
    using System;
    using System.ServiceModel;
    using Kernel.DependancyResolver;

    public class CustomServiceHost : ServiceHost
    {
        private readonly IDependencyResolver _container;

		public CustomServiceHost(IDependencyResolver container, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            this._container = container;
        }

        protected override void OnOpening()
        {
            Description.Behaviors.Add(new CustomServiceBehavior(_container));
            base.OnOpening();
        }
    }
}
