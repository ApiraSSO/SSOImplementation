using System.Collections.Generic;

namespace ORMMetadataContextProvider.Models
{
    public class SSODescriptorSettings : RoleDescriptorSettings
    {
        public SSODescriptorSettings()
        {
            this.LogoutServices = new List<EndPointSetting>();
        }
        public virtual ICollection<EndPointSetting> LogoutServices { get; }
        public virtual ICollection<IndexedEndPointSetting> ArtifactServices { get; }

    }
}