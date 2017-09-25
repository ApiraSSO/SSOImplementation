using System;
using System.Collections.Generic;
using Kernel.Data;
using Kernel.Federation.MetaData.Configuration.Cryptography;
using Kernel.Federation.MetaData.Configuration.Organisation;

namespace Kernel.Federation.MetaData.Configuration.RoleDescriptors
{
    public class RoleDescriptorConfiguration : IHasID<string>
    {
        public Type RoleDescriptorType { get; set; }
        public string Id { get; }
        public DateTimeOffset ValidUntil { get; set; }
        public TimeSpan CacheDuration { get; set; }
        public ICollection<Uri> ProtocolSupported { get; }
        public ICollection<KeyDescriptorConfiguration> KeyDescriptors { get; }
        public OrganisationConfiguration Organisation { get; set; }
        public Uri ErrorUrl { get; set; }
        public RoleDescriptorConfiguration()
        {
            this.ProtocolSupported = new List<Uri>();
            this.KeyDescriptors = new List<KeyDescriptorConfiguration>();
        }
    }
}