using Kernel.Federation.MetaData;

namespace WsFederationMetadataProvider.Metadata
{
    public class SingleSignOnServiceContext : ISingleSignOnServiceContext
    {
        public string Location { get; set; }

        public string Binding { get; set; }
    }
}