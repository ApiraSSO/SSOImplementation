using System;
using Kernel.Federation.MetaData.Configuration;

namespace Kernel.Federation.RelyingParty
{
    public class TenantContext
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
        public string RelyingPartyId { get; }
        public MetadataContext MetadataContext { get; set; }
        public TimeSpan AutomaticRefreshInterval
        {
            get
            {
                return this._automaticRefreshInterval;
            }
            set
            {
                if (value < TenantContext.MinimumAutomaticRefreshInterval)
                    throw new ArgumentOutOfRangeException("value", String.Format("IDX10107: When setting AutomaticRefreshInterval, the value must be greater than MinimumAutomaticRefreshInterval: '{0}'. value: '{1}'.", TenantContext.MinimumAutomaticRefreshInterval, value));
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
                if (value < TenantContext.MinimumRefreshInterval)
                    throw new ArgumentOutOfRangeException("value", String.Format("IDX10106: When setting RefreshInterval, the value must be greater than MinimumRefreshInterval: '{0}'. value: '{1}'.", TenantContext.MinimumRefreshInterval, value));
                this._refreshInterval = value;
            }
        }

        public TenantContext(string relyingPartyId, string metadataAddress)
        {
            if (String.IsNullOrWhiteSpace(relyingPartyId))
                throw new ArgumentNullException("relyingParty");

            if (String.IsNullOrWhiteSpace(metadataAddress))
                throw new ArgumentNullException("metadataContext");
            this.RelyingPartyId = relyingPartyId;
            this.MetadataAddress = metadataAddress;
            this.AutomaticRefreshInterval = TenantContext.DefaultAutomaticRefreshInterval;
            this.RefreshInterval = TenantContext.DefaultRefreshInterval;
        }
    }
}