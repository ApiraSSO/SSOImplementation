using System.Collections.Generic;

namespace Kernel.Federation.MetaData.Configuration.Cryptography
{
    public class MetadataSigningContext
    {
        public ICollection<KeyDescriptorConfiguration> KeyDescriptors { get; }
        public string DigestAlgorithm { get; }

        public string SignatureAlgorithm { get; }

        public MetadataSigningContext(string signatureAlgorithm, string digestAlgorithm)
        {
            this.SignatureAlgorithm = signatureAlgorithm;
            this.DigestAlgorithm = digestAlgorithm;
            this.KeyDescriptors = new List<KeyDescriptorConfiguration>();
        }
    }
}