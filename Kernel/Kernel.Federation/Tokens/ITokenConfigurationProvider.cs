namespace Kernel.Federation.Tokens
{
    public interface ITokenConfigurationProvider<TConfiguration>
    {
        TConfiguration GetConfiguration(string partnerId);
        TConfiguration GetTrustedIssuersConfiguration();
    }
}