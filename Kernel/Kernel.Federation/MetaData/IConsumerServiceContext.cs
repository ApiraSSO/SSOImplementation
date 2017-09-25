namespace Kernel.Federation.MetaData
{
    public interface IConsumerServiceContext
    {
        int Index { get; set; }

        bool IsDefault { get; set; }

        string Location { get; set; }

        string Binding { get; set; }
    }
}