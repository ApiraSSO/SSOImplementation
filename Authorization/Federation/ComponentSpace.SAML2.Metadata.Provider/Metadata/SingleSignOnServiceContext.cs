using Kernel.Federation.MetaData;

namespace ComponentSpace.SAML2.Metadata.Provider.Metadata
{
    public class SingleSignOnServiceContext : ISingleSignOnServiceContext
    {
        public string Location { get; set; }

        public string Binding { get; set; }
    }
}
