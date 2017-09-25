using System;
using System.Collections.Generic;
using Kernel.Federation.Audience;

namespace Kernel.Federation.FederationConfiguration
{
    public interface IAllowedAudienceConfiguration
    {
        IEnumerable<Uri> AllowedAudienceUris { get; set; }

        AudienceUriMode AudienceMode { get; set; }
    }
}