namespace Kernel.Federation.Protocols.Response
{
    public interface ITokenHandlerConfigurationProvider
    {
        void Configuration(ITokenHandler handler);
    }
}