namespace Kernel.WCF
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using Kernel.DependancyResolver;

    public class CustomInstanceProvider : IInstanceProvider
    {
        protected readonly Type _serviceType;
		protected readonly IDependencyResolver dependancyResolver;

		public CustomInstanceProvider(IDependencyResolver container, Type serviceType)
        {
            this._serviceType = serviceType;
            this.dependancyResolver = container;
        }

        public virtual object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public virtual object GetInstance(InstanceContext instanceContext, Message message)
        {
            return dependancyResolver.Resolve(_serviceType);
        }

        public virtual void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
			var disposable = instance as IDisposable;
			if (disposable != null)
				disposable.Dispose();
        }
    }
}