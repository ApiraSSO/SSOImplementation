namespace Kernel.Federation.MetaData.Configuration
{
    public interface IMetadataContextProvider
    {
        MetadataContext GetContext(MetadataType metadataType);
    }
}