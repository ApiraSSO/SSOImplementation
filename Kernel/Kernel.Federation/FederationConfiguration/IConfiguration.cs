using System;
using System.Security;

namespace Kernel.Federation.FederationConfiguration
{
    public interface IConfiguration
    {
        string IdpId { get; }

        int CacheExparation { get; }

        bool IsMetaDataSigned { get; set; }

        bool IsResponseSigned { get; set; }

        bool IsAssertionSigned { get; set; }

        bool IsRequestSigned { get; set; }

        bool RegisterSqlDepenencyMonitor { get; set; }

        bool RegisterFileDepenencyMonitor { get; set; }

        bool RequestHasClientId { get; set; }

        Func<Uri> GetAssertionServiceUri { get; set; }

        Func<string, IRelayState> GetRelayStateFromCacheFunc { get; set; }

        Func<IRelayState, int, string> AddRelayStateToCacheFunc { get; set; }

        Func<int> RelayStateCacheExparation { get; set; }

        string MetadataDbSourceConnectionString { get; }

        string SertificatePath { get; set; }

        SecureString SertificatePassword { get; set; }

        IAllowedAudienceConfiguration AllowedAudienceConfiguration { get; set; }
    }
}