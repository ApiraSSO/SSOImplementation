namespace Kernel.Federation.MetaData
{
    public interface ISingleSignOnServiceContext
    {
        string Location { get; set; }

        string Binding { get; set; }
    }
}