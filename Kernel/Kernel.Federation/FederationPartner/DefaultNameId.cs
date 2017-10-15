using System;

namespace Kernel.Federation.FederationPartner
{
    public class DefaultNameId
    {
        public DefaultNameId(Uri nameIdFormat)
        {
            this.NameIdFormat = nameIdFormat;
        }
        public bool AllowCreate { get; set; }
        public bool EncryptNameId { get; set; }
        public Uri NameIdFormat { get; }
    }
}