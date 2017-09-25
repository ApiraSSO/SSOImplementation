namespace ORMMetadataContextProvider.Models
{
    public class IndexedEndPointSetting : EndPointSetting
    {
        public int Index { get; set; }
        public bool IsDefault { get; set; }
    }
}