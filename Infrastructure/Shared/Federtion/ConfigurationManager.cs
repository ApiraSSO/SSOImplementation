using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Kernel.Extensions;
using Kernel.Federation.FederationPartner;

namespace Shared.Federtion
{
    public class ConfigurationManager<T> : IConfigurationManager<T> where T : class
    {
        private static ConcurrentDictionary<string, T> _congigurationCache = new ConcurrentDictionary<string, T>();
        private readonly SemaphoreSlim _refreshLock;
        private readonly IConfigurationRetriever<T> _configRetriever;
        private readonly IFederationPartyContextBuilder _federationPartyContextBuilder;
        
        public ConfigurationManager(IFederationPartyContextBuilder federationPartyContextBuilder, IConfigurationRetriever<T> configRetriever)
        {
            if (federationPartyContextBuilder == null)
                throw new ArgumentNullException("context");

            if (configRetriever == null)
                throw new ArgumentNullException("configRetriever");
            
            this._federationPartyContextBuilder = federationPartyContextBuilder;
            this._configRetriever = configRetriever;
            this._refreshLock = new SemaphoreSlim(1);
        }

        public async Task<T> GetConfigurationAsync(string federationPartyId)
        {
            T configuration = await this.GetConfigurationAsync(federationPartyId, CancellationToken.None)
                .ConfigureAwait(true);
            return configuration;
        }

        public async Task<T> GetConfigurationAsync(string federationPartyId, CancellationToken cancel)
        {
            var context = this._federationPartyContextBuilder.BuildContext(federationPartyId);
           
            var currentConfiguration = await this.GetConfiguration(context, cancel);
            
            return currentConfiguration;
        }

        public void RequestRefresh(string federationPartyId)
        {
            var context = this._federationPartyContextBuilder.BuildContext(federationPartyId);
            var utcNow = DateTimeOffset.UtcNow;
            if (!(utcNow >= DataTimeExtensions.Add(context.LastRefresh.UtcDateTime, context.RefreshInterval)))
                return;
            context.SyncAfter = utcNow;
        }

        private async Task<T> GetConfiguration(FederationPartyConfiguration context, CancellationToken cancel)
        {
            var now = DateTimeOffset.UtcNow;

            T currentConfiguration;
            if (ConfigurationManager<T>._congigurationCache.TryGetValue(context.FederationPartyId, out currentConfiguration))
            {
                if (context.SyncAfter > now)
                    return currentConfiguration;
            }
           
            await this._refreshLock.WaitAsync(cancel);
            try
            {
                if (context.SyncAfter <= now)
                {
                    try
                    {
                        ConfigurationManager<T> configurationManager = this;
                        
                        T obj = await this._configRetriever.GetAsync(context, CancellationToken.None)
                            .ConfigureAwait(true);
                        currentConfiguration = obj;
                        
                        configurationManager = null;
                        obj = default(T);

                        context.LastRefresh = now;
                        context.SyncAfter = DataTimeExtensions.Add(now.UtcDateTime, context.AutomaticRefreshInterval);
                    }
                    catch (Exception ex)
                    {
                        context.SyncAfter = DataTimeExtensions.Add(now.UtcDateTime, context.AutomaticRefreshInterval < context.RefreshInterval ? context.AutomaticRefreshInterval : context.RefreshInterval);
                        throw new InvalidOperationException(String.Format("IDX10803: Unable to obtain configuration from: '{0}'.", (context.MetadataAddress ?? "null")), ex);
                    }
                }

                ConfigurationManager<T>._congigurationCache.TryAdd(context.FederationPartyId, currentConfiguration);
                return currentConfiguration;
            }
            finally
            {
                this._refreshLock.Release();
            }
        }
    }
}