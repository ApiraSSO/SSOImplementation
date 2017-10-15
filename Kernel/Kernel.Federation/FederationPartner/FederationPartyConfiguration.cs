using System;
using Kernel.Federation.MetaData.Configuration;

namespace Kernel.Federation.FederationPartner
{
    public class FederationPartyConfiguration
    {
        public static readonly TimeSpan DefaultAutomaticRefreshInterval = new TimeSpan(1, 0, 0, 0);
        public static readonly TimeSpan DefaultRefreshInterval = new TimeSpan(0, 0, 0, 30);
        public static readonly TimeSpan MinimumAutomaticRefreshInterval = new TimeSpan(0, 0, 5, 0);
        public static readonly TimeSpan MinimumRefreshInterval = new TimeSpan(0, 0, 0, 1);

        private DateTimeOffset _syncAfter = DateTimeOffset.MinValue;
        private DateTimeOffset _lastRefresh = DateTimeOffset.MinValue;
        private TimeSpan _automaticRefreshInterval;
        private TimeSpan _refreshInterval;
        public DateTimeOffset SyncAfter
        {
            get
            {
                return this._syncAfter;
            }
            set
            {
                this._syncAfter = value;
            }
        }

        public DateTimeOffset LastRefresh
        {
            get
            {
                return this._lastRefresh;
            }
            set
            {
                this._lastRefresh = value;
            }
        }
        public string MetadataAddress { get; }
        public string FederationPartyId { get; }
        public MetadataContext MetadataContext { get; set; }
        public FederationPartyAuthnRequestConfiguration FederationPartyAuthnRequestConfiguration { get; set; }
        public TimeSpan AutomaticRefreshInterval
        {
            get
            {
                return this._automaticRefreshInterval;
            }
            set
            {
                if (value < FederationPartyConfiguration.MinimumAutomaticRefreshInterval)
                    throw new ArgumentOutOfRangeException("value", String.Format("IDX10107: When setting AutomaticRefreshInterval, the value must be greater than MinimumAutomaticRefreshInterval: '{0}'. value: '{1}'.", FederationPartyConfiguration.MinimumAutomaticRefreshInterval, value));
                this._automaticRefreshInterval = value;
            }
        }
        public TimeSpan RefreshInterval
        {
            get
            {
                return this._refreshInterval;
            }
            set
            {
                if (value < FederationPartyConfiguration.MinimumRefreshInterval)
                    throw new ArgumentOutOfRangeException("value", String.Format("IDX10106: When setting RefreshInterval, the value must be greater than MinimumRefreshInterval: '{0}'. value: '{1}'.", FederationPartyConfiguration.MinimumRefreshInterval, value));
                this._refreshInterval = value;
            }
        }
        public Uri DefaultNameIdFormat { get; set; }
        
        public ScopingConfiguration ScopingConfiguration { get; set; }

        public FederationPartyConfiguration(string federationPartyId, string metadataAddress)
        {
            if (String.IsNullOrWhiteSpace(federationPartyId))
                throw new ArgumentNullException("federationParty");

            if (String.IsNullOrWhiteSpace(metadataAddress))
                throw new ArgumentNullException("metadataContext");
            this.FederationPartyId = federationPartyId;
            this.MetadataAddress = metadataAddress;
            this.AutomaticRefreshInterval = FederationPartyConfiguration.DefaultAutomaticRefreshInterval;
            this.RefreshInterval = FederationPartyConfiguration.DefaultRefreshInterval;
            this.DefaultNameIdFormat = new Uri("urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified");
            this.ScopingConfiguration = new ScopingConfiguration();
        }
        public AuthnRequestConfiguration GetRequestConfigurationFromContext()
        {
            if (this.MetadataContext == null)
                throw new ArgumentNullException("metadataContext");

            return new AuthnRequestConfiguration(this.MetadataContext.EntityDesriptorConfiguration, this.DefaultNameIdFormat, this.ScopingConfiguration, this.FederationPartyAuthnRequestConfiguration);
        }
    }
}