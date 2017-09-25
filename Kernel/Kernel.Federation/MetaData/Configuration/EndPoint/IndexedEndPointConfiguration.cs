namespace Kernel.Federation.MetaData.Configuration.EndPoint
{
    public class IndexedEndPointConfiguration : EndPointConfiguration
    {
        public int Index { get; set; }
        public bool? IsDefault { get; set; }
    }
}