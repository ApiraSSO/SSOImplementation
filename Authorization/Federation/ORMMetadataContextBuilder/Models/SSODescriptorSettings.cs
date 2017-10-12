using System.Collections.Generic;

namespace ORMMetadataContextProvider.Models
{
    public class SSODescriptorSettings : RoleDescriptorSettings
    {
        public SSODescriptorSettings()
        {
            this.LogoutServices = new List<EndPointSetting>();
            this.NameIdFormats = new List<NameIdFormat>();
            this.ArtifactServices = new List<IndexedEndPointSetting>();
        }
        public virtual ICollection<EndPointSetting> LogoutServices { get; }
        public virtual ICollection<IndexedEndPointSetting> ArtifactServices { get; }
        public virtual ICollection<NameIdFormat> NameIdFormats { get; }

    }
}