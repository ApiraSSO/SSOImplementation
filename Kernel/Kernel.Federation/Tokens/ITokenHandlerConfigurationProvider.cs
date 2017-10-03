namespace Kernel.Federation.Tokens
{
    public interface ITokenHandlerConfigurationProvider
    {
        void Configuration(ITokenHandler handler, string partnerId);
    }
}