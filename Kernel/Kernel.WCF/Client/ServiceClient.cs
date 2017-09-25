using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Kernel.WCF.Client
{
    public class ServiceClient<TLocalType> : IDisposable where TLocalType : class
    {
        private bool _disposed;

        private readonly TLocalType _serviceInstance;

        private readonly ChannelFactory<TLocalType> _channel;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public TLocalType Instance
        {
            get
            {
                return _serviceInstance;
            }
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            try
            {
                if (_serviceInstance != null && _channel != null &&
                    _channel.State != CommunicationState.Faulted &&
                    _channel.State != CommunicationState.Closed)
                {
                    _channel.Close();
                }
            }
            catch
            {
                // just ignore - usually means channel as already been closed but state doesn't always report it.
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceClient&lt;TLocalType&gt;"/> class.
        /// </summary>
        public ServiceClient()
        {
            _channel = new ChannelFactory<TLocalType>(typeof(TLocalType).FullName);
            _serviceInstance = _channel.CreateChannel();
            _channel.Faulted += ServiceInstance_Faulted;
        }

        public ServiceClient(string endpointConfigurationName, IEnumerable<IEndpointBehavior> endpointBehaviours = null)
        {
            _channel = new ChannelFactory<TLocalType>(endpointConfigurationName);

            if (endpointBehaviours != null)
            {
                foreach (var b in endpointBehaviours)
                {
                    _channel.Endpoint.EndpointBehaviors.Add(b);
                }
            }

            _serviceInstance = _channel.CreateChannel();

            _channel.Faulted += ServiceInstance_Faulted;
        }

        public ServiceClient(string endpointConfiguration, EndpointAddress address)
        {
            _channel = new ChannelFactory<TLocalType>(endpointConfiguration, address);
            _serviceInstance = _channel.CreateChannel();
            _channel.Faulted += ServiceInstance_Faulted;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceClient{TLocalType}"/> class.
        /// </summary>
        /// <param name="customEndPointBehaviour">The custom end point behaviour.</param>
        public ServiceClient(IEndpointBehavior customEndPointBehaviour)
        {
            _channel = new ChannelFactory<TLocalType>(typeof(TLocalType).FullName);
            _channel.Endpoint.Behaviors.Add(customEndPointBehaviour);
            _serviceInstance = _channel.CreateChannel();
            _channel.Faulted += ServiceInstance_Faulted;
        }

        /// <summary>
        /// Handles the Faulted event of the ServiceInstance control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ServiceInstance_Faulted(object sender, EventArgs e)
        {
            if (_channel != null)
            {
                _channel.Abort();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Close();
                }
                _disposed = true;
            }
        }
    }
}