using System.Threading.Tasks;
using Kernel.DependancyResolver;
using Shared.Initialisation;

namespace FileSystemMetadataWriter.Initialisation
{
    public class FileSystemMetadataWriterInitialiser : Initialiser
    {
        public override byte Order
        {
            get { return 1; }
        }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<MetadataFileWriter>(Lifetime.Transient);
            
            return Task.CompletedTask;
        }
    }
}