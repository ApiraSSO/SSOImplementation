using System.Collections.Generic;
using Kernel.Data.ORM;

namespace ORMMetadataContextProvider.Seeders
{
    internal abstract class Seeder : ISeeder
    {
        internal const string CertificatesKey = "certificates";
        internal const string ProtocolsKey = "protocols";
        internal const string BindingsKey = "bindings";
        internal const string SPDescriptorsKey = "spDescriptor";
        internal const string Organisation = "organisation";
        internal const string EntityDescriptor = "entityDescriptor";
        internal const string Signing = "signing";
        internal const string Metadata = "metadata";
        internal const string Security = "security";
        internal const string NameIdKey = "nameIdFormat";
        internal const string SamlAutnContextKey = "samlAutnContext";

        internal static IDictionary<string, object> _cache = new Dictionary<string, object>();

        public virtual string ClientIdentifier { get; }

        public virtual byte SeedingOrder { get { return 0; } }

        public abstract void Seed(IDbContext context);
    }
}